using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Sixeyed.Caching.Configuration;

namespace Sixeyed.Caching.Caches.TableStorage
{
    public class CachedEntity : TableEntity
    {
        public string SerializedItem {get; set;}

        public DateTime? ExpiresAt { get; set; }

        public CachedEntity() { }

        public CachedEntity(string cacheKey)
        {
            PartitionKey = DefaultPartitionKey;
            RowKey = cacheKey;
        }

        public static string DefaultPartitionKey
        {
            get { return CacheConfiguration.Current.DefaultCacheName; }
        }
    }
}
