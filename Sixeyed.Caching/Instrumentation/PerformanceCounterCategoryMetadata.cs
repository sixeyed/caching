using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sixeyed.Caching.Instrumentation
{
    /// <summary>
    /// Metadata for a performance counter category, containing cache performance counters
    /// </summary>
    public class PerformanceCounterCategoryMetadata
    {
        public PerformanceCounterMetadata CacheRequests { get; private set; }
        public PerformanceCounterMetadata CacheHits { get; private set; }
        public PerformanceCounterMetadata CacheMisses { get; private set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<PerformanceCounterMetadata> Counters { get; private set; }

        public PerformanceCounterCategoryMetadata()
        {
            CacheRequests = new PerformanceCounterMetadata()
            {
                Name = "Cache Requests",
                Description = "Total requests made to cache",
                Type = PerformanceCounterType.NumberOfItems64,
                Category = this
            };
            CacheHits = new PerformanceCounterMetadata()
            {
                Name = "Cache Hits",
                Description = "Total cache hits",
                Type = PerformanceCounterType.NumberOfItems64,
                Category = this
            };
            CacheMisses = new PerformanceCounterMetadata()
            {
                Name = "Cache Misses",
                Description = "Total cache misses",
                Type = PerformanceCounterType.NumberOfItems64,
                Category = this
            };
            Counters = new List<PerformanceCounterMetadata>() { CacheRequests, CacheHits, CacheMisses };
        }

        public const string DefaultName = "Sixeyed Cache";
        public const string DefaultDescription = "Sixeyed.Caching Cache counters";
    }
}