using Sixeyed.Caching.Configuration;

namespace Sixeyed.Caching.Instrumentation.PerformanceCounters
{
    /// <summary>
    /// Generic framework cache counters
    /// </summary>
    public static class FxCounters
    {
        public static PerformanceCounterCategoryMetadata CacheTotal { get; private set; }

        static FxCounters()
        {
            CacheTotal = new PerformanceCounterCategoryMetadata()
            {
                Name = CacheConfiguration.Current.PerformanceCounters.CategoryNamePrefix + " - Totals",
                Description = PerformanceCounterCategoryMetadata.DefaultDescription
            };
        }
    }
}