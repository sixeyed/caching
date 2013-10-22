using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Extensions;
using System.Threading;
using Sixeyed.Caching;
using Sixeyed.Caching.Tests.Stubs;
using Sixeyed.Caching.Serialization;
using Sixeyed.Caching.Extensions;

namespace Sixeyed.Caching.Tests.Serialization
{
    [TestClass]
    public class JsonSerializerTests
    {
        [TestMethod]
        public void Serialize()
        {
            var obj = StubRequestWithEnum.GetRequest();
            var expected = string.Format(Json.InstanceFormat, "{", obj.Id, obj.Name, obj.CreatedOn.ToString("yyyy-MM-dd"), Enum.GetName(typeof(Status), obj.Status), "}");
            var actual = Serializer.Json.Serialize(obj);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Deserialize()
        {
            var obj = Serializer.Json.Deserialize<StubRequestWithEnum>(Json.Instance);
            Assert.IsNotNull(obj);
            Assert.AreEqual(194198183, obj.Id);
            Assert.AreEqual("91aa9c6b-4a0d-4b3f-9269-6bb6868a26ff", obj.Name);
            Assert.AreEqual(new DateTime(2010,10,16), obj.CreatedOn);
            Assert.AreEqual(Status.InUse, obj.Status);
        }

        private struct Json
        {
            public const string InstanceFormat = @"{0}""Id"":{1},""Name"":""{2}"",""CreatedOn"":""{3}T00:00:00Z"",""Status"":""{4}""{5}";
            public const string Instance = @"{""Id"":194198183,""Name"":""91aa9c6b-4a0d-4b3f-9269-6bb6868a26ff"",""CreatedOn"":""2010-10-16T00:00:00Z"",""Status"":""InUse""}";
        }
    }
}
