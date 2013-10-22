using System;
using Sixeyed.Caching;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Sixeyed.Caching.Serialization;

namespace Sixeyed.Caching.Containers.Interception.Cache
{
    /// <summary>
    /// Interception attribute for methods where responses are cached
    /// </summary>
    /// <remarks>
    /// Settings applied using <see cref="CacheAttribute"/> can be overridden at run-time
    /// by using the <see cref="CacheConfiguration"/> settings
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CacheAttribute : HandlerAttribute
    {
        /// <summary>
        /// Lifespan of the response in the cache
        /// </summary>
        public TimeSpan Lifespan
        {
            get { return new TimeSpan(Days, Hours, Minutes, Seconds); }
        }

        /// <summary>
        /// Whether caching is enabled
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Days the element to be cached should live in the cache
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Hours the element to be cached should live in the cache
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// Minutes the element to be cached should live in the cache
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Seconds the items should live in the cache
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// The type of cache required for the item
        /// </summary>
        public CacheType CacheType { get; set; }

        /// <summary>
        /// The type of serialization used for the cache key and cached item
        /// </summary>
        public SerializationFormat SerializationFormat { get; set; }

        /// <summary>
        /// Creates a <see cref="CacheCallHandler"/> to intercept invocations
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var handler = new CacheCallHandler {Order = Order};
            return handler;
        }
    }
}