using Enyim.Caching;
using Enyim.Caching.Memcached;
using Sixeyed.Caching.Logging;
using System;

namespace Sixeyed.Caching.Caches
{
    public class MemcachedCache : CacheBase
    {
        private MemcachedClient _cache;

        public override CacheType CacheType
        {
            get { return CacheType.Memcached; }
        }

        protected override void InitialiseInternal()
        {
            if (_cache == null)
            {
                Log.Debug("MemcachedCache.Initialise - initialising");
                _cache = new MemcachedClient();
            }
        }

        protected override void SetInternal(string key, object value)
        {
            _cache.Store(StoreMode.Set, key, value);
        }

        protected override void SetInternal(string key, object value, DateTime expiresAt)
        {
            _cache.Store(StoreMode.Set, key, value, expiresAt);
        }

        protected override void SetInternal(string key, object value, TimeSpan validFor)
        {
            _cache.Store(StoreMode.Set, key, value, validFor);
        }

        protected override object GetInternal(string key)
        {
            return _cache.Get(key);
        }

        protected override void RemoveInternal(string key)
        {
            if (Exists(key))
            {
                _cache.Remove(key);
            }
        }

        protected override bool ExistsInternal(string key)
        {
            return GetInternal(key) != null;
        }
    }
}
