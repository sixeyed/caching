
using System.Configuration;
using Sixeyed.Caching;
using Sixeyed.Caching.Serialization;

namespace Sixeyed.Caching.Configuration
{
    /// <summary>
    /// Element for configuring cache targets
    /// </summary>
    public class CacheTargetElement : ConfigurationElement
    {
        /// <summary>
        /// Returns the key for the cache target
        /// </summary>
        [ConfigurationProperty(SettingName.KeyPrefix)]
        public string KeyPrefix
        {
            get { return (string)this[SettingName.KeyPrefix]; }
        }

        /// <summary>
        /// Returns whether caching is enabled for the target
        /// </summary>
        [ConfigurationProperty(SettingName.Enabled, DefaultValue=true)]
        public bool Enabled
        {
            get { return (bool)this[SettingName.Enabled]; }
        }

        /// <summary>
        /// Gets the number of Days to cache the target value
        /// </summary>
        [ConfigurationProperty(SettingName.Days, DefaultValue=0)]
        public int Days
        {
            get { return (int)this[SettingName.Days]; }
        }

        /// <summary>
        /// Gets the number of Hours to cache the target value
        /// </summary>
        [ConfigurationProperty(SettingName.Hours, DefaultValue = 0)]
        public int Hours
        {
            get { return (int)this[SettingName.Hours]; }
        }

        /// <summary>
        /// Gets the number of Minutes to cache the target value
        /// </summary>
        [ConfigurationProperty(SettingName.Minutes, DefaultValue = 0)]
        public int Minutes
        {
            get { return (int)this[SettingName.Minutes]; }
        }

        /// <summary>
        /// Gets the number of Seconds to cache the target value
        /// </summary>
        [ConfigurationProperty(SettingName.Seconds, DefaultValue = 0)]
        public int Seconds
        {
            get { return (int)this[SettingName.Seconds]; }
        }

        /// <summary>
        /// Gets the type of cache for the target value
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="CacheType.InProcess"/>
        /// </remarks>
        [ConfigurationProperty(SettingName.CacheType, DefaultValue = CacheType.Memory)]
        public CacheType CacheType
        {
            get { return (CacheType)this[SettingName.CacheType]; }
        }

        /// <summary>
        /// The type of serialization used for the cache key and cached item
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="SerializationFormat.Json"/>
        /// </remarks>
        [ConfigurationProperty(SettingName.SerializationFormat, DefaultValue = SerializationFormat.Json)]
        public SerializationFormat SerializationFormat { get; set; }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// keyPrefix
            /// </summary>
            public const string KeyPrefix = "keyPrefix";

            /// <summary>
            /// enabled
            /// </summary>
            public const string Enabled = "enabled";
            
            
            /// <summary>
            /// days
            /// </summary>
            public const string Days = "days";
            
            /// <summary>
            /// hours
            /// </summary>
            public const string Hours = "hours";
            
            /// <summary>
            /// minutes
            /// </summary>
            public const string Minutes = "minutes";
            
            /// <summary>
            /// seconds
            /// </summary>
            public const string Seconds = "seconds";

            /// <summary>
            /// cacheType
            /// </summary>
            public const string CacheType = "cacheType";

            /// <summary>
            /// serializationFormat
            /// </summary>
            public const string SerializationFormat = "serializationFormat";
        }
    }
}

