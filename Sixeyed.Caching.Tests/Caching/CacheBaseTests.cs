using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Sixeyed.Caching.Tests.Caching
{
    /// <summary>
    /// Tests the OutputCacheProvider behaviour of CacheBase
    /// </summary>
    /// <remarks>
    /// Flexes the output cache by calling the stub website, 
    /// configured to use DiskCache
    /// </remarks>
    [TestClass]
    public class CacheBaseTests
    {
        [TestMethod]
        public void MvcOutputCache()
        {
            var client = new WebClient();
            var stopwatch = Stopwatch.StartNew();
            client.DownloadString("http://localhost/Sixeyed.Caching.Tests.Stubs.Website/Page1");
            //first fetch, page has 2sec sleep:
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > 2000);
            //next fetch is cached:
            stopwatch = Stopwatch.StartNew();
            client.DownloadString("http://localhost/Sixeyed.Caching.Tests.Stubs.Website/Page1");
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 2000);
        }

        [TestMethod]
        public void WebFormsOutputCache()
        {
            var client = new WebClient();
            var stopwatch = Stopwatch.StartNew();
            client.DownloadString("http://localhost/Sixeyed.Caching.Tests.Stubs.Website/Page2.aspx");
            //first fetch, page has 2sec sleep:
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > 2000);
            //next fetch is cached:
            stopwatch = Stopwatch.StartNew();
            client.DownloadString("http://localhost/Sixeyed.Caching.Tests.Stubs.Website/Page2.aspx");
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 2000);
        }

        [TestInitialize]
        [TestCleanup]
        public void ClearDownCache()
        {
            var path = @"c:\cache\website";
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
