using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sixeyed.Caching.Tests.Stubs
{
    [Serializable] //required for direct use of NCacheExpress
    [DataContract] //for sensible serialization with DataContractSerialier
    public class StubRequestWithEnum
    {
        private static Random _random = new Random();

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public Status Status { get; set; }

        public static StubRequestWithEnum GetRequest()
        {
            var req = new StubRequestWithEnum();
            req.Id = _random.Next();
            req.Name = Guid.NewGuid().ToString();
            req.CreatedOn = DateTime.UtcNow.Date.AddDays(_random.Next(100, 1000) * -1);
            req.Status = _random.Next() % 2 == 0 ? Status.Deprecated : Status.InUse;
            return req;
        }
    }

    [DataContract]
    public enum Status
    {
        [EnumMember]
        Null = 0,

        [EnumMember]
        InUse,

        [EnumMember]
        Deprecated
    }
}
