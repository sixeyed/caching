using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using Moq;

namespace Sixeyed.Caching.Tests.Stubs
{
    public class MethodInvocationStub
    {
        private static Random _random = new Random();

        public virtual void StubMethod(StubRequest request, int count) { }

        public virtual void NoParmsMethod() { }

        public static IMethodInvocation GetNullParametersProxyMock()
        {
            var stub = new MethodInvocationStubProxy();
            var method = typeof(MethodInvocationStubProxy).GetMethod("NoParmsMethod");
            IParameterCollection parms = null;

            var mi = new Mock<IMethodInvocation>();
            mi.Setup(x => x.MethodBase).Returns(method);
            mi.Setup(x => x.Target).Returns(stub);
            mi.Setup(x => x.Inputs).Returns(parms);
            return mi.Object;
        }

        public static IMethodInvocation GetNullValueProxyMock(int count)
        {
            var stub = new MethodInvocationStubProxy();
            var method = typeof(MethodInvocationStubProxy).GetMethod("StubMethod");
            var parms = new ParameterCollection(new object[] { null, count }, method.GetParameters(), p => true);

            var mi = new Mock<IMethodInvocation>();
            mi.Setup(x => x.MethodBase).Returns(method);
            mi.Setup(x => x.Target).Returns(stub);
            mi.Setup(x => x.Inputs).Returns(parms);
            return mi.Object;
        }

        public static IMethodInvocation GetProxyMock(StubRequest req = null, int? count = null)
        {
            var stub = new MethodInvocationStubProxy();
            var method = typeof(MethodInvocationStubProxy).GetMethod("StubMethod");
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

        public static IMethodInvocation GetMock(StubRequest req = null, int? count = null)
        {
            var stub = new MethodInvocationStub();
            var method = typeof(MethodInvocationStub).GetMethod("StubMethod");
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
    }

    public class MethodInvocationStubProxy : MethodInvocationStub { }
}
