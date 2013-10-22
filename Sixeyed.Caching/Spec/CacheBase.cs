using Sixeyed.Caching.Caches;
using Sixeyed.Caching.Configuration;
using Sixeyed.Caching.Cryptography;
using Sixeyed.Caching.Logging;
using Sixeyed.Caching.Serialization;
using System;
using System.Web.Caching;

namespace Sixeyed.Caching
{
    /// <summary>
    /// Base class for <see cref="ICache"/> implementations, also implements <see cref="OutputCacheProvider"/>
    /// </summary>
    public abstract class CacheBase : OutputCacheProvider, ICache
    {
        private static NullCache _nullCache = new NullCache();
        private CacheBase _current;
        private bool _initialised;

        protected CacheBase Current
        {
            get 
            {
                if (!CacheConfiguration.Current.Enabled)
                {
                    return _nullCache;
                }
                if (!_initialised)
                {
                    Initialise();
                    _initialised = true;
                }
                return this;
            }
        }

        public abstract CacheType CacheType { get; }
        protected abstract void InitialiseInternal();
        protected abstract void SetInternal(string key, object value);
        protected abstract void SetInternal(string key, object value, DateTime expiresAt);
        protected abstract void SetInternal(string key, object value, TimeSpan validFor);
        protected abstract object GetInternal(string key);
        protected abstract void RemoveInternal(string key);
        protected abstract bool ExistsInternal(string key);

        protected virtual bool ItemsNeedSerializing 
        {
            get { return false; ; }
        }

        public void Initialise()
        {
            try
            {
                InitialiseInternal();
            }
            catch (Exception ex)
            {
                Log.Error("CacheBase.Initialise - failed, NullCache will be used. CacheName: {0}, Message: {1}", CacheConfiguration.Current.DefaultCacheName, ex.Message);
                _current = _nullCache;
            }
        }

        public void Set(string key, object value, SerializationFormat serializationFormat = SerializationFormat.None)
        {
            try
            {
                value = PreProcess(value, serializationFormat);
                Current.SetInternal(key, value);
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Set - failed, item not cached. Message: {0}", ex.Message);
            }
        }

        void ICache.Set(string key, object value, DateTime expiresAt, SerializationFormat serializationFormat = SerializationFormat.Null)
        {
            try
            {
                value = PreProcess(value, serializationFormat);
                Current.SetInternal(key, value, expiresAt);
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Set - failed, item not cached. Message: {0}", ex.Message);
            }
        }

        public void Set(string key, object value, TimeSpan validFor, SerializationFormat serializationFormat = SerializationFormat.Null)
        {
            try
            {
                value = PreProcess(value, serializationFormat);
                Current.SetInternal(key, value, validFor);
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Set - failed, item not cached. Message: {0}", ex.Message);
            }
        }

        public T Get<T>(string key, SerializationFormat serializationFormat = SerializationFormat.Null)
        {
            return (T)Get(typeof(T), key, serializationFormat);
        }

        public object Get(Type type, string key, SerializationFormat serializationFormat = SerializationFormat.Null)
        {
            object item = null;
            try
            {
                item = Current.GetInternal(key);
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Get - failed, item not cached. Message: {0}", ex.Message);
            }
            return PostProcess(type, item, serializationFormat);
        }

        public override void Remove(string key)
        {
            try
            {
                Current.RemoveInternal(key);
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Remove - failed, item not cached. Message: {0}", ex.Message);
            }
        }

        public bool Exists(string key)
        {
            var exists = false;
            try
            {
                exists = Current.ExistsInternal(key);
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Exists - failed, item not cached. Message: {0}", ex.Message);
            }
            return exists;
        }

        private object PreProcess(object value, SerializationFormat requestedFormat)
        {
            object processed = value;
            var doEncryption = CacheConfiguration.Current.Encryption.Enabled;
            var serializer = GetSerializer(requestedFormat, doEncryption);
            processed = serializer.Serialize(value);
            if (doEncryption)
            {
                processed = Encryption.Encrypt((string)processed);
            }
            return processed;
        }

        private object PostProcess(Type type, object value, SerializationFormat requestedFormat)
        {
            if (value == null)
                return value;

            var processed = value;
            var doEncryption = CacheConfiguration.Current.Encryption.Enabled;
            if (doEncryption)
            {
                processed = Encryption.Decrypt((string)processed);
            }
            var serializer = GetSerializer(requestedFormat, doEncryption);
            processed = serializer.Deserialize(type, processed);
            return processed;
        }

        private ISerializer GetSerializer(SerializationFormat requestedFormat, bool doEncryption)
        {
            var serializer = Serializer.GetCurrent(SerializationFormat.None);
            //if we're encrypting, we need to use a known serializer; 
            //otherwise use requested or configured setting
            if (doEncryption)
            {
                serializer = Serializer.Json;
            }
            else if (requestedFormat != SerializationFormat.Null)
            {
                serializer = Serializer.GetCurrent(requestedFormat);
            }
            else if (ItemsNeedSerializing)
            {
                serializer = Serializer.GetCurrent(CacheConfiguration.Current.DefaultSerializationFormat);
            }
            return serializer;
        }

        #region OutputCacheProvider implementation

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            Set(key, entry, utcExpiry);
            return entry;
        }

        public override void Set(string key, object value, DateTime expiresAt)
        {
            try
            {
                var itemBytes = Serializer.Binary.Serialize(value) as byte[];
                var item = new CacheItem() { ItemBytes = itemBytes };
                var serializedItem = Serializer.Json.Serialize(item);
                var cacheKey = CacheKeyBuilder.GetCacheKey(key);
                Current.SetInternal(cacheKey, serializedItem, expiresAt);
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Set - failed, item not cached. Message: {0}", ex.Message);
            }
        }

        public override object Get(string key)
        {
            var cacheKey = CacheKeyBuilder.GetCacheKey(key);
            object item = null;
            try
            {
                var serializedItem = Current.GetInternal(cacheKey);
                if (serializedItem != null)
                {
                    var cacheItem = Serializer.Json.Deserialize(typeof(CacheItem), serializedItem) as CacheItem;
                    item = Serializer.Binary.Deserialize(null, cacheItem.ItemBytes);
                }
            }
            catch (Exception ex)
            {
                Log.Warn("CacheBase.Get - failed, item not cached. Message: {0}", ex.Message);
            }
            return item;
        }

        #endregion
    }
}
