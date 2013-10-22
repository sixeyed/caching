using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Logging;
using System.IO;
using System.Linq;

namespace Sixeyed.Caching.Tests.Logging
{
    [TestClass]
    public class LogTests
    {
        [TestMethod]
        public void Debug()
        {
            var guid1 = Guid.NewGuid();
            Log.Debug("guid1: {0}", guid1);
            AssertLatestLogEntry("[DEBUG] guid1: " + guid1);
        }

        [TestMethod]
        public void Info()
        {
            var guid1 = Guid.NewGuid();
            Log.Info("guid1: {0}", guid1);
            AssertLatestLogEntry("[INFO] guid1: " + guid1);
        }

        [TestMethod]
        public void Warn()
        {
            var guid1 = Guid.NewGuid();
            Log.Warn("guid1: {0}", guid1);
            AssertLatestLogEntry("[WARN] guid1: " + guid1);
        }

        [TestMethod]
        public void Error()
        {
            var guid1 = Guid.NewGuid();
            Log.Error("guid1: {0}", guid1);
            AssertLatestLogEntry("[ERROR] guid1: " + guid1);
        }

        [TestMethod]
        public void ErrorWithException()
        {
            var ex = new DivideByZeroException();
            var guid1 = Guid.NewGuid();
            Log.Error(ex, "guid1: {0}", guid1);
            AssertLatestLogEntry("[ERROR] guid1: " + guid1 + ": System.DivideByZeroException: Attempted to divide by zero.");
        }

        [TestMethod]
        public void Fatal()
        {
            var guid1 = Guid.NewGuid();
            Log.Fatal("guid1: {0}", guid1);
            AssertLatestLogEntry("[FATAL] guid1: " + guid1);
        }

        private void AssertLatestLogEntry(string expected)
        {
            var log = File.ReadAllLines("Sixeyed.Caching.Tests.log");
            var entry = log.Last();
            Assert.AreEqual(expected, entry);
        }
    }
}
