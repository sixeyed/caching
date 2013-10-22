using System.Linq;
using Sixeyed.Caching.Containers;

namespace Sixeyed.Caching.Serialization
{
    /// <summary>
    /// Wrapper for accessing <see cref="ISerializer"/> implementations
    /// </summary>
    public static class Serializer
    {
        public static ISerializer GetCurrent(SerializationFormat format)
        {            
            var serializers = Container.GetAll<ISerializer>();
            return (from s in serializers
                    where s.Format == format
                    select s).FirstOrDefault();
        }

        public static ISerializer Json     
        {
            get { return GetCurrent(SerializationFormat.Json); }
        }

        public static ISerializer Xml
        {
            get { return GetCurrent(SerializationFormat.Xml); }
        }

        public static ISerializer Binary
        {
            get { return GetCurrent(SerializationFormat.Binary); }
        }
    }
}
