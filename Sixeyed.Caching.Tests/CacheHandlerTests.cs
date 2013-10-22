using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bupa.BPI.Fx.Containers.Interception.Cache;
using Bupa.BPI.Fx.Extensions;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bupa.BPI.Fx.Caching;
using Moq;
using Bupa.BPI.Fx.Serialization;
using Bupa.BPI.Fx.TestUtilities.Context;
using Bupa.BPI.Fx.Containers;
using Bupa.BPI.Fx.Containers.Lifetime;
using Bupa.BPI.Fx.TestUtilities.DataGenerators;

namespace Sixeyed.Caching.Tests.Caching
{
    [TestClass]
    public class CacheHandlerTests
    {
        public struct ServiceName
        {
            public const string XmlStub = "XmlServiceStub.GetGroupIdForQuote";
            public const string WcfStub = "WcfServiceStub.GetMemberDetails";
        }

        [TestMethod]
        public void InvokeCacheHandlerTest()
        {
            var testCache = new MockCaching();
            int x = testCache.MockAdd(10, 20);
        }

        [TestMethod]
        public void XmlService_CachedResponse()
        {
            Container.Register<XmlServiceStub>(Lifetime.Transient);
            SharedContext.ResetServiceCallCount(ServiceName.XmlStub);
            var stub = Container.Get<XmlServiceStub>();
            Assert.IsNotNull(stub);
            var request = new GetGroupIdForQuoteRequest();
            var occ1 = RandomDataGenerator.GetRandomString(10);
            request.OccupationCode = occ1;            
            var response = stub.GetGroupIdForQuote(request);
            Assert.AreEqual(1, SharedContext.GetServiceCallCount(ServiceName.XmlStub));
            Assert.AreEqual("GROUP-" + occ1, response);
            //call again, ensure cached:
            response = stub.GetGroupIdForQuote(request);
            Assert.AreEqual(1, SharedContext.GetServiceCallCount(ServiceName.XmlStub));
            Assert.AreEqual("GROUP-" + occ1, response);
            //call with new request, ensure not cached:
            var occ2 = RandomDataGenerator.GetRandomString(10);
            request.OccupationCode = occ2;          
            response = stub.GetGroupIdForQuote(request);
            Assert.AreEqual(2, SharedContext.GetServiceCallCount(ServiceName.XmlStub));
            Assert.AreEqual("GROUP-" + occ2, response);
            //call with req 1:
            request.OccupationCode = occ1;
            response = stub.GetGroupIdForQuote(request);
            Assert.AreEqual(2, SharedContext.GetServiceCallCount(ServiceName.XmlStub));
            Assert.AreEqual("GROUP-" + occ1, response);
            //call with req 2:
            request.OccupationCode = occ2;
            response = stub.GetGroupIdForQuote(request);
            Assert.AreEqual(2, SharedContext.GetServiceCallCount(ServiceName.XmlStub));
            Assert.AreEqual("GROUP-" + occ2, response);
        }

        [TestMethod]
        public void WcfService_CachedResponse()
        {
            Container.Register<WcfServiceStub>(Lifetime.Transient);
            SharedContext.ResetServiceCallCount(ServiceName.WcfStub);
            var stub = Container.Get<WcfServiceStub>();
            Assert.IsNotNull(stub);
            var request = new GetMemberDetailsRequest();
            var id1 = RandomDataGenerator.GetRandomString(10);
            request.MemberId = id1;            
            var response = stub.GetMemberDetails(request);
            Assert.AreEqual(1, SharedContext.GetServiceCallCount(ServiceName.WcfStub));
            Assert.AreEqual("MEMBER-" + id1, response);
            //call again, ensure cached:
            response = stub.GetMemberDetails(request);
            Assert.AreEqual(1, SharedContext.GetServiceCallCount(ServiceName.WcfStub));
            Assert.AreEqual("MEMBER-" + id1, response);
            //call with new request, ensure not cached:
            var id2 = RandomDataGenerator.GetRandomString(10);
            request.MemberId = id2;
            response = stub.GetMemberDetails(request);
            Assert.AreEqual(2, SharedContext.GetServiceCallCount(ServiceName.WcfStub));
            Assert.AreEqual("MEMBER-" + id2, response);
            //call with req 1:
            request.MemberId = id1;
            response = stub.GetMemberDetails(request);
            Assert.AreEqual(2, SharedContext.GetServiceCallCount(ServiceName.WcfStub));
            Assert.AreEqual("MEMBER-" + id1, response);
            //call with req 2:
            request.MemberId = id2;
            response = stub.GetMemberDetails(request);
            Assert.AreEqual(2, SharedContext.GetServiceCallCount(ServiceName.WcfStub));
            Assert.AreEqual("MEMBER-" + id2, response);
        }
    }

    public class MockCaching
    {
        [Cache(CacheType=CacheType.InProcess,Days=0,Disabled=true,Hours=0,Minutes=0,Order=1,Seconds=0)]
        public int MockAdd(int x, int y)
        {
            Cache.GetCurrent(CacheType.InProcess);
            return x + y;
        }
    }

    public class XmlServiceStub
    {
        [Cache(SerializationFormat = SerializationFormat.Xml)]
        public virtual string GetGroupIdForQuote(GetGroupIdForQuoteRequest request)
        {
            SharedContext.IncrementServiceCallCount(CacheHandlerTests.ServiceName.XmlStub);
            return string.Format("GROUP-{0}", request.OccupationCode);
        }
    }

    #region XML service contracts

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://bupa/ukm/swift/Sales/SalesService")]
    public partial class GetGroupIdForQuoteRequest
    {

        private string occupationCodeField;

        private string voucherCodeField;

        private System.DateTime policyStartDateField;

        /// <remarks/>
        public string OccupationCode
        {
            get
            {
                return this.occupationCodeField;
            }
            set
            {
                this.occupationCodeField = value;
            }
        }

        /// <remarks/>
        public string VoucherCode
        {
            get
            {
                return this.voucherCodeField;
            }
            set
            {
                this.voucherCodeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime PolicyStartDate
        {
            get
            {
                return this.policyStartDateField;
            }
            set
            {
                this.policyStartDateField = value;
            }
        }
    }

    #endregion

    public class WcfServiceStub
    {
        [Cache]
        public virtual string GetMemberDetails(GetMemberDetailsRequest request)
        {
            SharedContext.IncrementServiceCallCount(CacheHandlerTests.ServiceName.WcfStub);
            return string.Format("MEMBER-{0}", request.MemberId);
        }
    }

#region WCF data contracts

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "GetMemberDetailsRequest", Namespace = "http://schemas.datacontract.org/2004/07/Bupa.BHW.Integration.Swift.Members.IBLFac" +
        "ade.RequestDTOs")]
    [System.SerializableAttribute()]
    public partial class GetMemberDetailsRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MemberIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RegistrationIdField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MemberId
        {
            get
            {
                return this.MemberIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MemberIdField, value) != true))
                {
                    this.MemberIdField = value;
                    this.RaisePropertyChanged("MemberId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RegistrationId
        {
            get
            {
                return this.RegistrationIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.RegistrationIdField, value) != true))
                {
                    this.RegistrationIdField = value;
                    this.RaisePropertyChanged("RegistrationId");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
#endregion
}
