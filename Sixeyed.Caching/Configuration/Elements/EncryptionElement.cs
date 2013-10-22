using System.Configuration;

namespace Sixeyed.Caching.Configuration
{
    public class EncryptionElement : ConfigurationElement
    {
        [ConfigurationProperty(SettingName.Enabled, DefaultValue=false)]
        public bool Enabled
        {
            get { return (bool)this[SettingName.Enabled]; }
        }

        [ConfigurationProperty(SettingName.Key, DefaultValue="")]
        public string Key
        {
            get { return (string)this[SettingName.Key]; }
        }

        [ConfigurationProperty(SettingName.InitializationVector, DefaultValue="")]
        public string InitializationVector
        {
            get { return (string)this[SettingName.InitializationVector]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// Enabled
            /// </summary>
            public const string Enabled = "enabled";

            /// <summary>
            /// key
            /// </summary>
            public const string Key = "key";

            /// <summary>
            /// iv
            /// </summary>
            public const string InitializationVector = "iv";
        }
    }
}
