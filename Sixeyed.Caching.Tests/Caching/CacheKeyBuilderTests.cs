using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Practices.Unity.InterceptionExtension;
using Sixeyed.Caching.Containers.Interception.Cache;
using Sixeyed.Caching;
using Sixeyed.Caching.Tests.Stubs;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class CacheKeyBuilderTests
    {
        private Random _random = new Random();

        [TestMethod]
        public void GetCacheKey()
        {
            var format = "{0}_{1}";
            var p1 = Guid.NewGuid().ToString();
            var p2 = _random.Next();
            var key1 = CacheKeyBuilder.GetCacheKey(format, p1, p2);
            AssertGuid(key1);
            //same input should generate same key:
            var key1v2 = CacheKeyBuilder.GetCacheKey(format, p1, p2);
            Assert.AreEqual(key1, key1v2);
            //different input should generate different key:
            var p3 = _random.Next();
            var key2 = CacheKeyBuilder.GetCacheKey(format, p1, p3);
            AssertGuid(key2);
            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void GetCacheKey_MethodInvocation()
        {
            var invocation = MethodInvocationStub.GetProxyMock();
            var key1 = CacheKeyBuilder.GetCacheKey(invocation);
            Assert.IsTrue(key1.StartsWith("MethodInvocationStub.StubMethod"));
            AssertGuid(key1, true);
            //same input should generate same key:
            var key1v2 = CacheKeyBuilder.GetCacheKey(invocation);
            Assert.AreEqual(key1, key1v2);
            //different input should generate different key:
            invocation = MethodInvocationStub.GetProxyMock();
            var key2 = CacheKeyBuilder.GetCacheKey(invocation);
            Assert.IsTrue(key2.StartsWith("MethodInvocationStub.StubMethod"));
            AssertGuid(key2, true);
            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void GetCacheKey_MethodInvocation_CustomPrefix()
        {            
            var invocation = CustomKeyPrefixMethodInvocationStub.GetMock();
            var key1 = CacheKeyBuilder.GetCacheKey(invocation);
            Assert.IsTrue(key1.StartsWith("*CustomKeyPrefix*"));
            AssertGuid(key1, true);
            //same input should generate same key:
            var key1v2 = CacheKeyBuilder.GetCacheKey(invocation);
            Assert.AreEqual(key1, key1v2);
            //different input should generate different key:
            invocation = CustomKeyPrefixMethodInvocationStub.GetMock();
            var key2 = CacheKeyBuilder.GetCacheKey(invocation);
            Assert.IsTrue(key2.StartsWith("*CustomKeyPrefix*"));
            AssertGuid(key2, true);
            Assert.AreNotEqual(key1, key2);
        }

        private static void AssertGuid(string value, bool hasPrefix = false)
        {            
            Assert.IsFalse(string.IsNullOrWhiteSpace(value));
            if (hasPrefix)
            {
                var parts = value.Split('_');
                Assert.AreEqual(2, parts.Length);
                value = parts[1];
            }
            var guid1 = Guid.Parse(value);
            Assert.IsNotNull(guid1);
            Assert.AreNotEqual(Guid.Empty, guid1);
        }
    }
}
