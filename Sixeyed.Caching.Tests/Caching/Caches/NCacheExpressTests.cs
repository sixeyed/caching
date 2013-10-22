using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Extensions;
using System.Threading;
using Sixeyed.Caching.Caching;
using Sixeyed.Caching.Tests.Stubs;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class NCacheExpressTests
    {
        [TestMethod]
        public void Set()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            Cache.NCacheExpress.Set(key, value);
            Assert.IsTrue(Cache.NCacheExpress.Exists(key));
            var retrievedValue = Cache.NCacheExpress.Get<StubRequest>(key);
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
            Cache.NCacheExpress.Set(key, value, expiresAt);
            Assert.IsTrue(Cache.NCacheExpress.Exists(key));
            Thread.Sleep(2000);
            Assert.IsFalse(Cache.NCacheExpress.Exists(key));
        }

        [TestMethod]
        public void Set_WithSlidingExpiry()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            var lifespan = new TimeSpan(0, 0, 0, 0, 250);
            Cache.NCacheExpress.Set(key, value, lifespan);
            Assert.IsTrue(Cache.NCacheExpress.Exists(key));
            Thread.Sleep(200);
            var retrieved = Cache.NCacheExpress.Get(key);
            Assert.IsNotNull(retrieved);
            Thread.Sleep(200);
            retrieved = Cache.NCacheExpress.Get(key);
            Assert.IsNotNull(retrieved);
            Thread.Sleep(2000);
            Assert.IsFalse(Cache.NCacheExpress.Exists(key));
        }

        [TestMethod]
        public void Set_ThenRemove()
        {
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            Cache.NCacheExpress.Set(key, value);
            Assert.IsTrue(Cache.NCacheExpress.Exists(key));
            Cache.NCacheExpress.Remove(key);
            Assert.IsFalse(Cache.NCacheExpress.Exists(key));
        }
    }
}
