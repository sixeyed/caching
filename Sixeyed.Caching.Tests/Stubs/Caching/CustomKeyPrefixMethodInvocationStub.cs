using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using Moq;
using Sixeyed.Caching.Containers.Interception.Cache;

namespace Sixeyed.Caching.Tests.Stubs
{
    public class CustomKeyPrefixMethodInvocationStub : ICacheKeyPrefixProvider
    {
        private static Random _random = new Random();

        public virtual void StubMethod(StubRequest request, int count) { }
                
        public static IMethodInvocation GetMock(StubRequest req = null, int? count = null)
        {
            var stub = new CustomKeyPrefixMethodInvocationStub();
            var method = typeof(CustomKeyPrefixMethodInvocationStub).GetMethod("StubMethod");
            if (req == null)
            {
                req = StubRequest.GetRequest();
            }
            if (count == null)
            {
                count = _random.Next();
            }
            var parms = new ParameterCollection(new object[] { req, count }, method.GetParameters(), p => true);

            var mi = new Mock<IMethodInvocation>();
            mi.Setup(x => x.MethodBase).Returns(method);
            mi.Setup(x => x.Target).Returns(stub);
            mi.Setup(x => x.Inputs).Returns(parms);
            return mi.Object;
        }

        public string GetCacheKeyPrefix()
        {
            return "*CustomKeyPrefix*";
        }
    }
}
