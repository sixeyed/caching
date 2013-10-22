using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Extensions;

namespace Sixeyed.Caching.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void FormatWith()
        {
            var format = "{0}_{1}";
            var arg1 = Guid.NewGuid();
            var arg2 = new Random().Next();
            var expected = arg1 + "_" + arg2;
            Assert.AreEqual(expected, format.FormatWith(arg1, arg2));
            format = Guid.NewGuid().ToString();
            expected = format;
            Assert.AreEqual(expected, format.FormatWith(null));
            Assert.AreEqual(expected, format.FormatWith(new object[] {}));
        }

        [TestMethod]
        public void IsNullOrEmpty()
        {
            var empty = string.Empty;
            Assert.IsTrue(empty.IsNullOrEmpty());
            empty = null;
            Assert.IsTrue(empty.IsNullOrEmpty());
            var notEmpty = Guid.NewGuid().ToString();
            Assert.IsFalse(notEmpty.IsNullOrEmpty());
            var whitespace = "    ";
            Assert.IsFalse(whitespace.IsNullOrEmpty());
        }

        [TestMethod]
        public void IsNullOrWhitespace()
        {
            var empty = string.Empty;
            Assert.IsTrue(empty.IsNullOrWhitespace());
            empty = null;
            Assert.IsTrue(empty.IsNullOrEmpty());
            var notEmpty = Guid.NewGuid().ToString();
            Assert.IsFalse(notEmpty.IsNullOrWhitespace());
            var whitespace = "    ";
            Assert.IsTrue(whitespace.IsNullOrWhitespace());
        }
    }
}
