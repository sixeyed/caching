using Sixeyed.Caching.Serialization;
using System;
using System.Collections.Generic;

namespace Sixeyed.Caching
{
    public interface ICache
    {
        /// <summary>
        /// Returns the type of the cache
        /// </summary>
        CacheType CacheType { get; }

        /// <summary>
        /// Performs initialisation tasks required for the cache implementation
        /// </summary>
        void Initialise();

        /// <summary>
        /// Insert or update a cache value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="serializationFormat"></param>
        void Set(string key, object value, SerializationFormat serializationFormat = SerializationFormat.Null);

        /// <summary>
        /// Insert or update a cache value with an expiry date
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresAt"></param>
        /// <param name="serializationFormat"></param>
        void Set(string key, object value, DateTime expiresAt, SerializationFormat serializationFormat = SerializationFormat.Null);

        /// <summary>
        /// Insert or update a cache value with a fixed lifetime
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="validFor"></param>
        /// <param name="serializationFormat"></param>
        void Set(string key, object value, TimeSpan validFor, SerializationFormat serializationFormat = SerializationFormat.Null);

        /// <summary>
        /// Retrieve a value from cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serializationFormat"></param>
        /// <returns>Cached value or null</returns>
        object Get(Type type, string key, SerializationFormat serializationFormat = SerializationFormat.Null);

        /// <summary>
        /// Retrieve a typed value from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="serializationFormat"></param>
        /// <returns></returns>
        T Get<T>(string key, SerializationFormat serializationFormat = SerializationFormat.Null);

        /// <summary>
        /// Removes the value for the given key from the cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Returns whether the cache contains a value for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);
    }
}
