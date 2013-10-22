using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sixeyed.Caching.Serialization.Serializers
{
    /// <summary>
    /// Binary implementaion of <see cref="ISerializer"/>
    /// </summary>
    public class BinarySerializer : ISerializer
    {
        public SerializationFormat Format
        {
            get { return SerializationFormat.Binary; }
        }

        public object Deserialize(Type type, object serializedValue)
        {
            object deserialized;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(serializedValue as byte[]))
            {
                 deserialized = formatter.Deserialize(stream);
            }
            return deserialized;
        }

        public object Serialize(object value)
        {
            byte[] serialized;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, value);
                stream.Flush();
                serialized = stream.ToArray();
            }
            return serialized;
        }
    }
}
