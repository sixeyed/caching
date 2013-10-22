using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Sixeyed.Caching.Serialization.Serializers
{
    /// <summary>
    /// JSON implementaion of <see cref="ISerializer"/>
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        public SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

        public object Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, GetSettings());
        }

        public object Deserialize(Type type, object serializedValue)
        {
            return JsonConvert.DeserializeObject((string)serializedValue, type, GetSettings());
        }

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters = new JsonConverter[] { new StringEnumConverter() };
            return settings;
        }
    }
}

