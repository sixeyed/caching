using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Tests.Stubs;
using System.Threading;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class CacheCallHandlerTests
    {
        [TestMethod]
        public void Invoke()
        {
            var value1 = new MethodLevelCachingStub().GetRandomInt();
            Assert.IsTrue(value1 > -1);
            var value2 = new MethodLevelCachingStub().GetRandomInt();
            Assert.AreEqual(value1, value2);
        }

        [TestMethod]
        public void Invoke_CacheDisabledInCode()
        {
            var value1 = new MethodLevelCachingStub().GetRandomIntUncached();
            Assert.IsTrue(value1 > -1);
            var value2 = new MethodLevelCachingStub().GetRandomIntUncached();
            Assert.AreNotEqual(value1, value2);
            Assert.IsTrue(value2 > -1);
        }

        [TestMethod]
        public void Invoke_CacheExpires()
        {
            var value1 = new MethodLevelCachingStub().GetRandomIntCacheExpires();
            Assert.IsTrue(value1 > -1);
            var value2 = new MethodLevelCachingStub().GetRandomIntCacheExpires();
            Assert.AreEqual(value1, value2);
            Thread.Sleep(200);
            var value3 = new MethodLevelCachingStub().GetRandomIntCacheExpires();
            Assert.AreEqual(value1, value3);
            Thread.Sleep(1000);
            var value4 = new MethodLevelCachingStub().GetRandomIntCacheExpires();
            Assert.AreNotEqual(value1, value4);
            Assert.IsTrue(value4 > -1);
        }

        [TestMethod]
        public void Invoke_CacheExpiryInConfig()
        {
            var value1 = new MethodLevelCachingStub().GetRandomIntCacheExpiresConfigured();
            Assert.IsTrue(value1 > -1);
            var value2 = new MethodLevelCachingStub().GetRandomIntCacheExpiresConfigured();
            Assert.AreEqual(value1, value2);
            Thread.Sleep(200);
            var value3 = new MethodLevelCachingStub().GetRandomIntCacheExpiresConfigured();
            Assert.AreEqual(value1, value3);
            Thread.Sleep(1000);
            var value4 = new MethodLevelCachingStub().GetRandomIntCacheExpiresConfigured();
            Assert.AreNotEqual(value1, value4);
            Assert.IsTrue(value4 > -1);
        }

        [TestMethod]
        public void Invoke_CacheDisabledInConfig()
        {
            var value1 = new MethodLevelCachingStub().GetRandomIntCacheConfigured();
            Assert.IsTrue(value1 > -1);
            var value2 = new MethodLevelCachingStub().GetRandomIntCacheConfigured();
            Assert.AreNotEqual(value1, value2);
            Assert.IsTrue(value2 > -1);
        }
    }
}
