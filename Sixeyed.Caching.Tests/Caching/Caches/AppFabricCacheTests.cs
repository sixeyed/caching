using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Extensions;
using System.Threading;
using Sixeyed.Caching;
using Sixeyed.Caching.Tests.Stubs;
using System.Diagnostics;
using System.Linq;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class AppFabricCacheTests
    {
        private ICache _cache = Cache.AppFabric;

        [TestMethod]
        public void Set()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            _cache.Set(key, value);
            Assert.IsTrue(_cache.Exists(key));
            var retrievedValue = _cache.Get<StubRequest>(key);
            Assert.AreEqual(value.CreatedOn, retrievedValue.CreatedOn);
            Assert.AreEqual(value.Id, retrievedValue.Id);
            Assert.AreEqual(value.Name, retrievedValue.Name);
        }

        [TestMethod]
        public void Set_WithAbsoluteExpiry()
        {
            AssertCacheIsRunning();
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            var expiresAt = DateTime.Now.AddMilliseconds(250);
            _cache.Set(key, value, expiresAt);
            Assert.IsTrue(_cache.Exists(key));
            Thread.Sleep(500);
            Assert.IsFalse(_cache.Exists(key));
        }

        [TestMethod]
        public void Set_WithTimeoutExpiry()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            var lifespan = new TimeSpan(0, 0, 0, 0, 250);
            _cache.Set(key, value, lifespan);
            Assert.IsTrue(_cache.Exists(key));
            Thread.Sleep(200);
            var retrieved = _cache.Get<StubRequest>(key);
            Assert.IsNotNull(retrieved);
            
            Thread.Sleep(200);
            retrieved = _cache.Get<StubRequest>(key);
            Assert.IsNull(retrieved);
            Assert.IsFalse(_cache.Exists(key));
        }

        [TestMethod]
        public void Set_ThenRemove()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            _cache.Set(key, value);
            Assert.IsTrue(_cache.Exists(key));
            _cache.Remove(key);
            Assert.IsFalse(_cache.Exists(key));
        }

        [TestInitialize]
        public void AssertCacheIsRunning()
        {
            var processes = Process.GetProcessesByName("DistributedCacheService");
            if (!processes.Any())
            {
                Assert.Inconclusive("AppFabric cache service not running locally");
            }
        }
    }
}
