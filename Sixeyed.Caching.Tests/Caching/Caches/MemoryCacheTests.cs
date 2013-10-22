using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Extensions;
using System.Threading;
using Sixeyed.Caching;
using Sixeyed.Caching.Tests.Stubs;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class MemoryCacheTests
    {
        [TestMethod]
        public void Set()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            Cache.Memory.Set(key, value);
            Assert.IsTrue(Cache.Memory.Exists(key));
            var retrievedValue = Cache.Memory.Get<StubRequest>(key);
            Assert.AreEqual(value.CreatedOn, retrievedValue.CreatedOn);
            Assert.AreEqual(value.Id, retrievedValue.Id);
            Assert.AreEqual(value.Name, retrievedValue.Name);
        }

        [TestMethod]
        public void Set_WithAbsoluteExpiry()
        {            
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            var expiresAt = DateTime.Now.AddMilliseconds(250);
            Cache.Memory.Set(key, value, expiresAt);
            Assert.IsTrue(Cache.Memory.Exists(key));
            Thread.Sleep(500);
            Assert.IsFalse(Cache.Memory.Exists(key));
        }

        // * Sliding expiration doesn't work until .NET 4.5:
        // * http://connect.microsoft.com/VisualStudio/feedback/details/644778/slidingexpiration-does-not-seem-to-work-on-memorycache             * 
        [Ignore]
        [TestMethod]
        public void Set_WithSlidingExpiry()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            var lifespan = new TimeSpan(0, 0, 0, 0, 250);
            Cache.Memory.Set(key, value, lifespan);
            Assert.IsTrue(Cache.Memory.Exists(key));
            Thread.Sleep(200);
            var retrieved = Cache.Memory.Get<StubRequest>(key);
            Assert.IsNotNull(retrieved);
            
            Thread.Sleep(200);
            retrieved = Cache.Memory.Get<StubRequest>(key);
            Assert.IsNotNull(retrieved);
            
            Thread.Sleep(500);            
            Assert.IsFalse(Cache.Memory.Exists(key));
        }

        [TestMethod]
        public void Set_ThenRemove()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            Cache.Memory.Set(key, value);
            Assert.IsTrue(Cache.Memory.Exists(key));
            Cache.Memory.Remove(key);
            Assert.IsFalse(Cache.Memory.Exists(key));
        }
    }
}
