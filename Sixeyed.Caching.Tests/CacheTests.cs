using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bupa.BPI.Fx.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bupa.BPI.Fx.Spec;

namespace Sixeyed.Caching.Tests.Caching
{    
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void GetCurrentTest()
        {
            var cache = Cache.GetCurrent(CacheType.OutOfProcess);
            AssertCache<OutOfProcessCache>(cache, CacheType.OutOfProcess);
            //repeat:
            cache = Cache.GetCurrent(CacheType.OutOfProcess);
            AssertCache<OutOfProcessCache>(cache, CacheType.OutOfProcess);
        }

        [TestMethod]
        public void InProcessTest()
        {
            AssertCache<InProcessCache>(Cache.InProcess, CacheType.InProcess);
        }

        [TestMethod]
        public void OutOfProcess()
        {
            AssertCache<OutOfProcessCache>(Cache.OutOfProcess, CacheType.OutOfProcess);
        }  

        private static void AssertCache<TCache>(ICache cache, CacheType cacheType)
        {
            Assert.IsNotNull(cache);
            Assert.IsInstanceOfType(cache, typeof(ICache));
            Assert.IsInstanceOfType(cache, typeof(TCache));
            Assert.AreEqual(cacheType, cache.CacheType);
        }              
    }
}
