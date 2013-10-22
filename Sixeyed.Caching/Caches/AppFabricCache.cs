using Microsoft.ApplicationServer.Caching;
using Sixeyed.Caching.Configuration;
using Sixeyed.Caching.Logging;
using System;

namespace Sixeyed.Caching.Caches
{
    public class AppFabricCache : CacheBase
    {
        private DataCacheFactory _factory;
        private DataCache _cache;

        public override CacheType CacheType
        {
            get {return CacheType.AppFabric; }
        }

        protected override void InitialiseInternal()
        {
            if (_cache == null)
            {
                var cacheName = CacheConfiguration.Current.DefaultCacheName.Replace(".", "-");
                Log.Debug("AppFabricCache.Initialise - initialising with cache name: {0}", cacheName);
                _factory = new DataCacheFactory();
                _cache = _factory.GetCache(cacheName);
            }
        }

        protected override void SetInternal(string key, object value)
        {
            _cache.Put(key, value);
        }

        protected override void SetInternal(string key, object value, DateTime expiresAt)
        {
            SetInternal(key, value, new TimeSpan(expiresAt.Subtract(DateTime.Now).Ticks));
        }

        protected override void SetInternal(string key, object value, TimeSpan validFor)
        {
            _cache.Put(key, value, validFor);
        }

        protected override object GetInternal(string key)
        {
            return _cache.Get(key);
        }

        protected override void RemoveInternal(string key)
        {
            _cache.Remove(key);
        }

        protected override bool ExistsInternal(string key)
        {
            return GetInternal(key) != null;
        }
    }
}
