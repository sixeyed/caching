using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Sixeyed.Caching.Logging;
using Sixeyed.Caching.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using unity = Microsoft.Practices.Unity;

namespace Sixeyed.Caching.Containers
{
    /// <summary>
    /// Container class wrapping <see cref="UnityContainer"/>
    /// </summary>
    /// <remarks>
    /// Resolves based on the first registration to the container, so registration can be 
    /// overridden at runtime using the <see cref="UnityConfigurationSection"/> settings
    /// </remarks>
    public static class Container
    {
        private static IUnityContainer _UnityContainer;

        static Container()
        {
            Initialise();
        }

        private static void Initialise()
        {
            _UnityContainer = new UnityContainer();
            _UnityContainer.AddNewExtension<unity.InterceptionExtension.Interception>();
            if (ConfigurationManager.GetSection("unity") != null)
            {
                _UnityContainer.LoadConfiguration();
            }
            var thisAssembly = typeof(ICache).Assembly;
            //caches:
            RegisterAll<ICache>(thisAssembly, Lifetime.Singleton);
            RegisterAll<ISerializer>(thisAssembly, Lifetime.Singleton);
        }

        /// <summary>
        /// Retrieve the default implementation of a registered type
        /// </summary>  
        public static T Get<T>()
        {
            var implementation = default(T);
            try
            {
                implementation = _UnityContainer.Resolve<T>();
            }
            catch (ResolutionFailedException ex)
            {
                //thrown if no implementations registered
                Log.Warn(
                    "Sixeyed.Caching.Containers.Container.Get<>() - resolution failed for type: {0}, message: {1}",
                    typeof(T).FullName, ex.Message);
            }
            return implementation;
        }

        /// <summary>
        /// Returns all registered implementations of a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>() where T : class
        {
            var implementations = new List<T>();
            implementations.AddRange(_UnityContainer.ResolveAll<T>());
            //check for a single registration:
            var implementation = Get<T>();
            if (implementation != null)
            {
                implementations.Add(implementation);
            }
            return implementations;
        }

        /// <summary>
        /// Register a type which will be resolved by the container
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="lifetime">Lifetime of resolved objects</param>
        public static void Register<T>(Lifetime lifetime)
        {
            if (!_UnityContainer.IsRegistered<T>())
            {
                _UnityContainer.RegisterType<T>(GetLifetimeManager(lifetime), new InjectionConstructor());
                _UnityContainer.Configure<Microsoft.Practices.Unity.InterceptionExtension.Interception>()
                    .SetInterceptorFor(typeof(T), new VirtualMethodInterceptor());
            }
        }

        /// <summary>
        /// Register an interface with an implementation to be resolved by the container
        /// </summary>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        public static void Register<TInterface, TImplementation>()
        {
            Register<TInterface, TImplementation>(Lifetime.Transient);
        }

        /// <summary>
        /// Register an interface with an implementation to be resolved by the container
        /// </summary>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="lifetime">Lifetime of the implementation</param>
        public static void Register<TInterface, TImplementation>(Lifetime lifetime)
        {
            if (!_UnityContainer.IsRegistered<TInterface>())
            {
                _UnityContainer.RegisterType(typeof(TInterface), typeof(TImplementation), GetLifetimeManager(lifetime),
                                               new InjectionConstructor());
                _UnityContainer.Configure<Microsoft.Practices.Unity.InterceptionExtension.Interception>()
                    .SetInterceptorFor(typeof(TImplementation), new VirtualMethodInterceptor());
            }
        }

        /// <summary>
        /// Register all implementations of a type which will be resolved by the container
        /// </summary>
        /// <remarks>
        /// Only registers one implementation per type, so calling with a base type will
        /// register a single implementation for each of the derived types in the assembly
        /// </remarks>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="assembly">Assembly containg types</param>
        public static void Register<T>(Assembly assembly)
        {
            Register<T>(assembly, Lifetime.Transient, false);
        }

        /// <summary>
        /// Register all implementations of a type which will be resolved by the container
        /// </summary>
        /// <remarks>
        /// Only registers one implementation per type, so calling with a base type will
        /// register a single implementation for each of the derived types in the assembly
        /// </remarks>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="assembly">Assembly containg types</param>
        /// <param name="lifetime">Lifetime of resolved objects</param>
        public static void Register<T>(Assembly assembly, Lifetime lifetime)
        {
            Register<T>(assembly, lifetime, false);
        }

        /// <summary>
        /// Register all implementations of a type in the assembly where the type is defined
        /// </summary>
        /// <remarks>
        /// Registers all implementations - so allows multiple registrations per type.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        public static void RegisterAll<T>()
        {
            RegisterAll<T>(typeof (T).Assembly, Lifetime.Transient);
        }

        /// <summary>
        /// Register all implementations of a type in the given assembly
        /// </summary>
        /// <remarks>
        /// Registers all implementations - so allows multiple registrations per type
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        public static void RegisterAll<T>(Assembly assembly)
        {
            RegisterAll<T>(assembly, Lifetime.Transient);
        }

        /// <summary>
        /// Register all implementations of a type in the given assembly
        /// </summary>
        /// <remarks>
        /// Registers all implementations - so allows multiple registrations per type
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="lifetime"></param>
        public static void RegisterAll<T>(Assembly assembly, Lifetime lifetime)
        {
            Register<T>(assembly, lifetime, true);
        }

        public static void RegisterInstance<T>(T instance, Lifetime lifetime = Lifetime.Thread)
        {
            if (!_UnityContainer.IsRegistered<T>())
            {
                _UnityContainer.RegisterInstance<T>(instance, GetLifetimeManager(lifetime));
            }
        }

        public static void UnregisterInstance<T>()
        {
            var registration = _UnityContainer.Registrations.Where(e => e.RegisteredType == typeof(T)).FirstOrDefault();
            if (registration != null)
            {
                registration.LifetimeManager.RemoveValue();
            }
        }

        private static void Register<T>(Assembly assembly, Lifetime lifetime, bool allowMultipleImplementations)
        {
            var fromType = typeof (T);
            var toTypes = from t in assembly.GetTypes()
                                        where t.IsClass
                                              && !t.IsAbstract
                                              && t.GetInterface(fromType.Name) != null
                                        select t;

            if (toTypes.Count() <= 0) return;            
            foreach (var toType in toTypes)
            {
                var manager = GetLifetimeManager(lifetime);
                var actualFromType = GetFromType(fromType, toType);
                if (allowMultipleImplementations)
                {
                    //ensure the same implementation is not re-registered:
                    if (!_UnityContainer.IsRegistered(actualFromType, toType.Name))
                    {
                        _UnityContainer.RegisterType(actualFromType, toType, toType.Name, manager, new InjectionConstructor());
                        _UnityContainer.Configure<Microsoft.Practices.Unity.InterceptionExtension.Interception>()
                            .SetInterceptorFor(toType, new VirtualMethodInterceptor());
                    }
                }
                else if (!_UnityContainer.IsRegistered(actualFromType))
                {
                    _UnityContainer.RegisterType(actualFromType, toType, manager, new InjectionConstructor());
                    _UnityContainer.Configure<Microsoft.Practices.Unity.InterceptionExtension.Interception>()
                        .SetInterceptorFor(toType, new VirtualMethodInterceptor());
                }
            }
        }

        private static Type GetFromType(Type fromType, Type toType)
        {
            if (fromType.IsInterface)
            {
                //check if the class implements derived interfaces:
                var derived = (from i in toType.GetInterfaces()
                                where i.GetInterface(fromType.Name) != null
                                select i).FirstOrDefault();
                if (derived != null)
                {
                    return derived;
                }
            }

            return fromType;
        }

        private static LifetimeManager GetLifetimeManager(Lifetime lifetime)
        {
            LifetimeManager manager = null;
            switch (lifetime)
            {
                case Lifetime.Transient:
                    manager = new TransientLifetimeManager();
                    break;
                case Lifetime.Singleton:
                    manager = new ContainerControlledLifetimeManager();
                    break;
                case Lifetime.Thread:
                    manager = new PerThreadLifetimeManager();
                    break;
                case Lifetime.CallContext:
                    manager = new PerCallContextLifeTimeManager();
                    break;
            }
            return manager;
        }
    }
}