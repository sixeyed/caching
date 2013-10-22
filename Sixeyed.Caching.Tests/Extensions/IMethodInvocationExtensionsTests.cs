using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sixeyed.Caching.Extensions;
using Sixeyed.Caching.Serialization;
using Sixeyed.Caching.Tests.Stubs;

namespace Sixeyed.Caching.Tests.Extensions
{
    [TestClass]
    public class IMethodInvocationExtensionsTests
    {
        private Random _random = new Random();

        [TestMethod]
        public void GetMethodCallPrefix()
        {
            var prefix = MethodInvocationStub.GetProxyMock().GetMethodCallPrefix();
            Assert.AreEqual("MethodInvocationStub.StubMethod ", prefix);
        }

        [TestMethod]
        public void ToTraceString()
        {
            var req = StubRequest.GetRequest();
            var count = _random.Next();
            var actual = MethodInvocationStub.GetProxyMock(req, count).ToTraceString();
            var expectedFormat = @"MethodInvocationStub.StubMethod request: {0}""Id"":{1},""Name"":""{2}"",""CreatedOn"":""{3}T00:00:00Z""{4}, count: {5}";
            var expected = string.Format(expectedFormat, "{", req.Id, req.Name, req.CreatedOn.ToString("yyyy-MM-dd"), "}", count);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToTraceString_Xml()
        {
            var req = StubRequest.GetRequest();
            var count = _random.Next();
            var actual = MethodInvocationStub.GetProxyMock(req, count).ToTraceString(Serializer.Xml);
            var expectedFormat = @"MethodInvocationStub.StubMethod <StubRequest xmlns=""http://schemas.datacontract.org/2004/07/Sixeyed.Caching.Tests.Stubs"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><CreatedOn>{0}T00:00:00Z</CreatedOn><Id>{1}</Id><Name>{2}</Name></StubRequest>_<int xmlns=""http://schemas.microsoft.com/2003/10/Serialization/"">{3}</int>";
            var expected = string.Format(expectedFormat, req.CreatedOn.ToString("yyyy-MM-dd"), req.Id, req.Name, count);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToTraceString_NullParameters()
        {
            var req = StubRequest.GetRequest();
            var count = _random.Next();
            var actual = MethodInvocationStub.GetNullParametersProxyMock().ToTraceString();
            Assert.AreEqual("MethodInvocationStub.NoParmsMethod", actual);
        }

        [TestMethod]
        public void ToTraceString_NullValue()
        {
            var count = _random.Next();
            var actual = MethodInvocationStub.GetNullValueProxyMock(count).ToTraceString();
            var expectedFormat = @"MethodInvocationStub.StubMethod request: [null], count: {0}";
            var expected = string.Format(expectedFormat, count);
            Assert.AreEqual(expected, actual);
        }
    }
}
