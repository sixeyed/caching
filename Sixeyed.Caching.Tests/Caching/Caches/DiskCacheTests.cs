using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Extensions;
using System.Threading;
using Sixeyed.Caching;
using Sixeyed.Caching.Tests.Stubs;
using Sixeyed.Caching.Serialization;
using System.IO;
using Sixeyed.Caching.Configuration;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class DiskCacheTests
    {
        private ICache _cache = Cache.Disk;

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
            //verify the item is encrypted:
            var cachedItem = File.ReadAllText(Path.Combine(CacheConfiguration.Current.DiskCache.Path, key + ".cache"));
            Assert.IsFalse(cachedItem.Contains(value.Name));
        }

        [TestMethod]
        public void Set_WithAbsoluteExpiry()
        {            
            var key = Guid.NewGuid().ToString();
            var value = StubRequest.GetRequest();
            //minimum expiry for disk cache is 1 second:
            var expiresAt = DateTime.Now.AddMilliseconds(1250);
            _cache.Set(key, value, expiresAt);
            Assert.IsTrue(_cache.Exists(key));
            Thread.Sleep(1500);
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
        [TestCleanup]
        public void ClearDownCache()
        {
            var path = CacheConfiguration.Current.DiskCache.Path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                foreach (var fileName in Directory.GetFiles(path))
                {
                    if (fileName.EndsWith(".cache") || fileName.EndsWith(".expiry"))
                    {
                        File.Delete(fileName);
                    }
                }
            }
        }
    }
}
