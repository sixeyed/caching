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
    public class XmlSerializerTests
    {
        [TestMethod]
        public void Serialize()
        {
            var obj = StubRequestWithEnum.GetRequest();
            var expected = string.Format(Xml.InstanceFormat, obj.CreatedOn.ToString("yyyy-MM-dd"), obj.Id, obj.Name, Enum.GetName(typeof(Status), obj.Status));
            var actual = Serializer.Xml.Serialize(obj);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Deserialize()
        {
            var obj = Serializer.Xml.Deserialize<StubRequestWithEnum>(Xml.Instance);
            Assert.IsNotNull(obj);
            Assert.AreEqual(732399822, obj.Id);
            Assert.AreEqual("e106b2df-370d-4eb0-b65c-3bbd42c1c26e", obj.Name);
            Assert.AreEqual(new DateTime(2011,01,23), obj.CreatedOn);
            Assert.AreEqual(Status.Deprecated, obj.Status);
        }

        private struct Xml
        {
            public const string InstanceFormat = @"<StubRequestWithEnum xmlns=""http://schemas.datacontract.org/2004/07/Sixeyed.Caching.Tests.Stubs"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><CreatedOn>{0}T00:00:00Z</CreatedOn><Id>{1}</Id><Name>{2}</Name><Status>{3}</Status></StubRequestWithEnum>";
            public const string Instance = @"<StubRequestWithEnum xmlns=""http://schemas.datacontract.org/2004/07/Sixeyed.Caching.Tests.Stubs"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><CreatedOn>2011-01-23T00:00:00Z</CreatedOn><Id>732399822</Id><Name>e106b2df-370d-4eb0-b65c-3bbd42c1c26e</Name><Status>Deprecated</Status></StubRequestWithEnum>";
        }
    }
}
