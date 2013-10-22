using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.Caching.Containers.Interception.Cache;
using Sixeyed.Caching.Containers;

namespace Sixeyed.Caching.Tests.Stubs
{
    public class MethodLevelCachingStub
    {
        static MethodLevelCachingStub()
        {
            Container.Register<MethodLevelCachingStub>(Lifetime.Transient);
        }       

        private static MethodLevelCachingStub Instance
        {
            get { return Container.Get<MethodLevelCachingStub>(); }
        }

        private static Random _random = new Random();

        public string TrickyMethod(bool returnNull, bool throwException)
        {
            return Instance.TrickyMethodInternal(returnNull, throwException);
        }

        public int GetRandomInt()
        {
            return Instance.GetRandomIntInternal();
        }

        public int GetRandomIntUncached()
        {
            return Instance.GetRandomIntUncachedInternal();
        }

        public int GetRandomIntCacheConfigured()
        {
            return Instance.GetRandomIntCacheConfiguredInternal();
        }

        public int GetRandomIntCacheExpires()
        {
            return Instance.GetRandomIntCacheExpiresInternal();
        }

        public int GetRandomIntCacheExpiresConfigured()
        {
            return Instance.GetRandomIntCacheExpiresConfiguredInternal();
        }

        [Cache]
        protected virtual int GetRandomIntInternal()
        {
            return _random.Next();
        }

        [Cache(Disabled=true)]
        protected virtual int GetRandomIntUncachedInternal()
        {
            return _random.Next();
        }

        [Cache]
        protected virtual int GetRandomIntCacheConfiguredInternal()
        {
            return _random.Next();
        }

        [Cache(Seconds=1)]
        protected virtual int GetRandomIntCacheExpiresInternal()
        {
            return _random.Next();
        }

        [Cache(Seconds = 10)]
        protected virtual int GetRandomIntCacheExpiresConfiguredInternal()
        {
            return _random.Next();
        }

        [Cache]
        protected virtual string TrickyMethodInternal(bool returnNull, bool throwException)
        {
            if (throwException)
            {
                throw new Exception();
            }
            if (returnNull)
            {
                return null;
            }
            return Instance.GetRandomIntInternal().ToString();
        }
    }
}
