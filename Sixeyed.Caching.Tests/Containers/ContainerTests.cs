using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Caching.Containers;
using Sixeyed.Caching.Tests.Stubs;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Sixeyed.Caching.Tests.Containers
{
    [TestClass]
    public class ContainerTests
    {
        [TestInitialize]
        public void ResetContainer()
        {
            var init = typeof(Container).GetMethod("Initialise", BindingFlags.NonPublic | BindingFlags.Static);
            init.Invoke(typeof(Container), null);
        }

        [TestMethod]
        public void NullReturnFromUnregisteredInterface()
        {
            Assert.IsNull(Container.Get<IOtherStub>());
        }

        [TestMethod]
        public void NullReturnFromNoImplmentedInterfaces()
        {
            Container.RegisterAll<IOtherStub>();
            Assert.IsNull(Container.Get<IOtherStub>());
        }

        [TestMethod]
        public void RegisterAllAllowsMultipleImplementations()
        {
            Container.RegisterAll<IContainedStub>();
            Assert.AreEqual(2, Container.GetAll<IContainedStub>().Count());
        }

        [TestMethod]
        public void FirstWinsInRepeatedRegisterAll()
        {
            Container.RegisterAll<IContainedStub>(typeof(IContainedStub).Assembly); //defaults to Transient
            Container.RegisterAll<IContainedStub>(typeof(IContainedStub).Assembly, Lifetime.Singleton);
            var stubs = Container.GetAll<IContainedStub>();
            Assert.AreEqual(2, stubs.Count());
            var stub1 = stubs.Where(x => x.GetType() == typeof(ContainedStub)).Single();
            var int1 = stub1.GetInt();
            Assert.AreEqual(int1, stub1.GetInt());
            stubs = Container.GetAll<IContainedStub>();
            var stub2 = stubs.Where(x => x.GetType() == typeof(ContainedStub)).Single();
            Assert.AreNotEqual(int1, stub2.GetInt());
        }

        [TestMethod]
        public void FirstWinsInRepeatRegistration()
        {
            Container.Register<IContainedStub, ContainedStub>();
            Container.Register<IContainedStub, OtherContainedStub>();
            Assert.IsInstanceOfType(Container.Get<IContainedStub>(), typeof(ContainedStub));
        }

        [TestMethod]
        public void FirstWinsInRepeatRegistrationFromAssembly()
        {
            Container.Register<IContainedStub, OtherContainedStub>();
            Container.Register<IContainedStub>(typeof(IContainedStub).Assembly, Lifetime.Thread);
            Assert.AreEqual(1, Container.GetAll<IContainedStub>().Count());
            Assert.IsInstanceOfType(Container.Get<IContainedStub>(), typeof(OtherContainedStub));
        }

        [TestMethod]
        public void RegisterDefaultsToTransient()
        {
            Container.Register<IContainedStub, ContainedStub>();
            var stub1 = Container.Get<IContainedStub>();
            var int1 = stub1.GetInt();
            Assert.AreEqual(int1, stub1.GetInt());
            var stub2 = Container.Get<IContainedStub>();            
            Assert.AreNotEqual(int1, stub2.GetInt());
        }

        [TestMethod]
        public void RegisterAndUnregisterInstance()
        {
            var stub1 = new ContainedStub();
            var int1 = stub1.GetInt();
            Container.RegisterInstance<IContainedStub>(stub1, Lifetime.CallContext);
            var stub2 = Container.Get<IContainedStub>();
            Assert.AreEqual(int1, stub2.GetInt());
            Container.UnregisterInstance<IContainedStub>();
            var stub3 = Container.Get<IContainedStub>();
            Assert.IsNull(stub3);
        }

        [TestMethod]
        public void RegisterWithCallContextIsReusedWithinTheThread()
        {
            Container.Register<IContainedStub, ContainedStub>(Lifetime.CallContext);
            var stub1 = Container.Get<IContainedStub>();
            var int1 = stub1.GetInt();
            Assert.AreEqual(int1, stub1.GetInt());
            var stub2 = Container.Get<IContainedStub>();
            Assert.AreEqual(int1, stub2.GetInt());
        }

        [TestMethod]
        public void RegisterWithCallContextLifetimeIsNotReusedAcrossThreads()
        {
            Container.Register<IContainedStub, ContainedStub>(Lifetime.CallContext);
            AssertAcrossThreads(false);
        }

        [TestMethod]
        public void RegisterWithThreadLifetimetIsNotReusedAcrossThreads()
        {
            Container.Register<IContainedStub, ContainedStub>(Lifetime.Thread);
            AssertAcrossThreads(false);
        }

        [TestMethod]
        public void RegisterWithSingletonLifetimeIsReusedAcrossThreads()
        {
            Container.Register<IContainedStub, ContainedStub>(Lifetime.Singleton);
            AssertAcrossThreads(true);
        }

        [TestMethod]
        public void RegisterBaseInterfaceRegistersDerivedInterfaces()
        {
            Container.Register<IRepository>(typeof(IRepository).Assembly);
            Assert.IsInstanceOfType(Container.Get<IUserRepository>(), typeof(UserRepositoryStub));
            Assert.IsInstanceOfType(Container.Get<IAccountRepository>(), typeof(AccountRepositoryStub));
        }

        [TestMethod]
        public void ConfigRegistrationOverridesCode()
        {
            Container.Register<IConfiguredStub, CodedStub>();
            Assert.IsInstanceOfType(Container.Get<IConfiguredStub>(), typeof(ConfiguredStub));
        }

        private static void AssertAcrossThreads(bool reused)
        {
            var stub1 = Container.Get<IContainedStub>();
            var int1 = stub1.GetInt();
            var ints = new ConcurrentBag<int>();
            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() => AddInt(ints)));
            tasks.Add(Task.Factory.StartNew(() => AddInt(ints)));
            Task.WaitAll(tasks.ToArray());
            foreach (var taskInt in ints)
            {
                if (reused)
                {
                    Assert.AreEqual(int1, taskInt);
                }
                else
                {
                    Assert.AreNotEqual(int1, taskInt);
                }
            }
        }

        private static void AddInt(ConcurrentBag<int> ints)
        {
            var stub = Container.Get<IContainedStub>();
            ints.Add(stub.GetInt());
        }
    }
}
