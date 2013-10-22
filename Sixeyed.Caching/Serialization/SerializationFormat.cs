

namespace Sixeyed.Caching.Serialization
{
    /// <summary>
    /// Defines serialization formats
    /// </summary>
    public enum SerializationFormat
    {
        /// <summary>
        /// No serialization format set
        /// </summary>
        Null = 0,

        /// <summary>
        /// No serailization to be done
        /// </summary>
        None,

        /// <summary>
        /// JSON serialization
        /// </summary>
        Json,

        /// <summary>
        /// XML serialization 
        /// </summary>
        Xml,

        /// <summary>
        /// Binary serialization
        /// </summary>
        Binary
    }
}
