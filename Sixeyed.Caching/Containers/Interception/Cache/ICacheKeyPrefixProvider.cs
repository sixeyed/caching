
namespace Sixeyed.Caching.Containers.Interception.Cache
{
    /// <summary>
    /// Represents a provider which can deliver a prefix to use for a cache key
    /// </summary>
    public interface ICacheKeyPrefixProvider
    {
        /// <summary>
        /// Returns a string to be used as a cache key prefix
        /// </summary>
        /// <returns></returns>
        string GetCacheKeyPrefix();
    }
}
