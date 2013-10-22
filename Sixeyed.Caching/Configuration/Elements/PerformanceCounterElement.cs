
using System.Configuration;
using Sixeyed.Caching;
using Sixeyed.Caching.Serialization;

namespace Sixeyed.Caching.Configuration
{
    public class PerformanceCounterElement : ConfigurationElement
    {
        [ConfigurationProperty(SettingName.InstrumentCacheTotalCounts)]
        public bool InstrumentCacheTotalCounts
        {
            get { return (bool)this[SettingName.InstrumentCacheTotalCounts]; }
        }

        [ConfigurationProperty(SettingName.InstrumentCacheTargetCounts)]
        public bool InstrumentCacheTargetCounts
        {
            get { return (bool)this[SettingName.InstrumentCacheTargetCounts]; }
        }

        [ConfigurationProperty(SettingName.CategoryNamePrefix, DefaultValue="Sixeyed Cache")]
        public string CategoryNamePrefix
        {
            get { return (string)this[SettingName.CategoryNamePrefix]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// instrumentCacheTotalCounts
            /// </summary>
            public const string InstrumentCacheTotalCounts = "instrumentCacheTotalCounts";

            /// <summary>
            /// instrumentCacheTargetCounts
            /// </summary>
            public const string InstrumentCacheTargetCounts = "instrumentCacheTargetCounts";

            public const string CategoryNamePrefix = "categoryNamePrefix";
        }
    }
}

