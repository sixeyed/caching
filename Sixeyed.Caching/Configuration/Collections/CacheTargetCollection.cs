using System.Configuration;

namespace Sixeyed.Caching.Configuration
{
    /// <summary>
    /// Collection of configured <see cref="CacheTargetElement"></see>
    ///   s
    /// </summary>
    [ConfigurationCollection(typeof (CacheTargetElement),
        AddItemName = "target",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CacheTargetCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Returns the element name used for serializing the collection
        /// </summary>
        protected override string ElementName
        {
            get { return "target"; }
        }

        /// <summary>
        /// Returns the collection type used (BasicMap)
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// Gets the regular expression configured with the specified name
        /// </summary>
        /// <param name="key">Name of regular expression to return</param>
        /// <returns><see cref="CacheTargetElement"/></returns>
        public new CacheTargetElement this[string key]
        {
            get { return (CacheTargetElement) BaseGet(key); }
        }

        /// <summary>
        /// Returns a new collection element
        /// </summary>
        /// <returns>New <see cref="CacheTargetElement"/></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CacheTargetElement();
        }

        /// <summary>
        /// Returns the collection key value
        /// </summary>
        /// <param name="element"><see cref="CacheTargetElement"/> element</param>
        /// <returns><see cref="CacheTargetElement"/> key value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CacheTargetElement) element).KeyPrefix;
        }
    }
}