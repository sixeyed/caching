using Sixeyed.Caching.Caches;
using Sixeyed.Caching.Configuration;
using Sixeyed.Caching.Containers;
using Sixeyed.Caching.Logging;
using System;
using System.Linq;

namespace Sixeyed.Caching
{
    /// <summary>
    /// Wrapper for accessing <see cref="ICache"/> implementations
    /// </summary>
    public static class Cache
    {
        public static ICache Get(CacheType cacheType)
        {
            ICache cache = new NullCache();
            try
            {
                var caches = Container.GetAll<ICache>();
                cache = (from c in caches
                         where c.CacheType == cacheType
                         select c).Last();
                cache.Initialise();
            }
            catch (Exception ex)
            {
                Log.Warn("Failed to instantiate cache of type: {0}, using null cache. Exception: {1}", cacheType, ex);
                cache = new NullCache();
            }
            return cache;
        }

        public static ICache Default
        {
            get
            {
                return Get(CacheConfiguration.Current.DefaultCacheType);
            }
        }

        public static ICache Memory
        {
            get
            {
                return Get(CacheType.Memory);
            }
        }

        public static ICache AppFabric
        {
            get
            {
                return Get(CacheType.AppFabric);
            }
        }

        public static ICache AzureTableStorage
        {
            get
            {
                return Get(CacheType.AzureTableStorage);
            }
        }

        public static ICache Disk
        {
            get
            {
                return Get(CacheType.Disk);
            }
        }

        public static ICache Memcached
        {
            get
            {
                return Get(CacheType.Memcached);
            }
        }
    }
}
