using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Extensions;
using System.Threading;
using Sixeyed.Caching;
using Sixeyed.Caching.Tests.Stubs;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class NullCacheTests
    {
        [TestMethod]
        public void Set()
        {
            var cache = Cache.Get(CacheType.Null);
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            cache.Set(key, value);
            Assert.IsFalse(cache.Exists(key));
            var retrievedValue = cache.Get<StubRequest>(key);
            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void Set_WithAbsoluteExpiry()
        {
            var cache = Cache.Get(CacheType.Null);
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            var expiresAt = DateTime.Now.AddMilliseconds(250);
            cache.Set(key, value, expiresAt);
            Assert.IsFalse(cache.Exists(key));
            var retrievedValue = cache.Get<StubRequest>(key);
            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void Set_WithSlidingExpiry()
        {
            var cache = Cache.Get(CacheType.Null);
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            var lifespan = new TimeSpan(0, 0, 0, 0, 250);
            cache.Set(key, value, lifespan);
            Assert.IsFalse(cache.Exists(key));
            var retrievedValue = cache.Get<StubRequest>(key);
            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void Set_ThenRemove()
        {
            var cache = Cache.Get(CacheType.Null);
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            cache.Set(key, value);
            Assert.IsFalse(cache.Exists(key));
            cache.Remove(key);
            Assert.IsFalse(cache.Exists(key));
        }
    }
}
