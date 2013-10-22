using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sixeyed.Caching.Configuration
{
    public class DiskCacheElement : ConfigurationElement
    {
        [ConfigurationProperty(SettingName.Path)]
        public string Path
        {
            get { return (string)this[SettingName.Path]; }
        }

        [ConfigurationProperty(SettingName.MaxSizeInMb, DefaultValue = 200)]
        public int MaxSizeInMb
        {
            get { return (int)this[SettingName.MaxSizeInMb]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// path
            /// </summary>
            public const string Path = "path";

            /// <summary>
            /// maxSize
            /// </summary>
            public const string MaxSizeInMb = "maxSizeInMb";
        }
    }
}
