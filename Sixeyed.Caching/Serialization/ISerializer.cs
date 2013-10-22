using System;

namespace Sixeyed.Caching.Serialization
{
    /// <summary>
    /// Represents a simple object serializer
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Format used by the serializer
        /// </summary>
        SerializationFormat Format { get; }

        /// <summary>
        /// Deserialize a serialized object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializedValue"></param>
        /// <returns></returns>
        object Deserialize(Type type, object serializedValue);

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        object Serialize(object value);
    }
}
