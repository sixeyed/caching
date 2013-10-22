
namespace Sixeyed.Caching
{
    public enum CacheType
    {
        /// <summary>
        /// No cache type set
        /// </summary>
        Null = 0,
        
        Memory,

        AppFabric,

        Memcached,

        AzureTableStorage,

        Disk
    }
}
