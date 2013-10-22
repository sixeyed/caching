
namespace Sixeyed.Caching.Containers
{
    /// <summary>
    /// Object lifetime for container resolution
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// New object instance created on every resolution
        /// </summary>
        Transient,

        /// <summary>
        /// Single object used for every resolution within an App Domain
        /// </summary>
        Singleton,

        /// <summary>
        /// Single object used for every resolution within a thread
        /// </summary>
        Thread,

        /// <summary>
        /// Single object used for every resolution within a call context - e.g. HTTP request
        /// </summary>
        CallContext
    }
}
