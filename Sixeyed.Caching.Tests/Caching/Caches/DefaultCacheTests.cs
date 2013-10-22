using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Caches;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class DefaultCacheTests
    {
        private ICache _cache = Cache.Default;

        [TestMethod]
        public void DefaultByConfiguration()
        {
            var cache = Cache.Default;
            Assert.IsInstanceOfType(cache, typeof(MemoryCache));
        }
    }
}
