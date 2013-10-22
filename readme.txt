Welcome to Sixeyed.Caching!

This is a framework library which wraps access to various .NET cache providers:

- MemoryCache
- AppFabric Cache
- Memcached

, and includes two custom cache implementations using persistent stores:

- DiskCache
- AzureTableStorageCache.

The library standardises cache access with an ICache interface, includes AOP
for method-result caching, ASP.NET caching for each of the providers, and takes
care of serializing and (optionally) encrypting cache items.

The library was developed and demonstrated in the Pluralsight course:

  * Caching in the .NET Stack: Inside-Out *

and is distributed with an MIT license, so is free to use in your own projects.

The source lives on GitHub:

  https://github.com/sixeyed/caching 
