using System;
using System.Collections.Generic;
using Bupa.BPI.Fx.Caching;
using Bupa.BPI.Fx.Spec;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class OutOfProcessCacheTests
    {
        [TestMethod]
        public void default_ctor_test()
        {
            new OutOfProcessCache();
        }

        [TestMethod]
        public void GetGroupTest()
        {
            var keys = new List<string> { "key1", "key2" };
            var cache = new OutOfProcessCache();

            foreach (var key in keys)
                cache.Set(key, key);

            var values = cache.GetGroup<string>(keys);            
            Assert.IsTrue(values.Count == 2);
        }

        [TestMethod]
        public void basic_put_and_get()
        {
            const string testKey = "TestKey";
            const string testValue = "TestValue";

            ICache cache = new OutOfProcessCache();
            cache.Remove(testKey);
            cache.Set(testKey, testValue);

            var actual = (string)cache.Get(testKey);

            Assert.AreEqual(testValue, actual);
            Assert.IsTrue(cache.Exists(testKey));
        }
        [TestMethod]
        public void expired_at_put()
        {
            const string testKey = "TestKey";
            const string testValue = "TestValue";

            ICache cache = new OutOfProcessCache();
            cache.Remove(testKey);
            cache.Set(testKey, testValue, DateTime.Now.AddMinutes(1));
            Assert.IsTrue(cache.Exists(testKey));
        }
        [TestMethod]
        public void expired_in_put()
        {
            const string testKey = "TestKey";
            const string testValue = "TestValue";

            ICache cache = new OutOfProcessCache();
            cache.Remove(testKey);
            cache.Set(testKey, testValue, new TimeSpan(0, 0, 1));
            Assert.IsTrue(cache.Exists(testKey));            
        }
    }
}
