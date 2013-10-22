using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sixeyed.Caching.Serialization;

namespace Sixeyed.Caching.Tests.Stubs
{
    [Serializable] //required for direct use of NCacheExpress
    [DataContract] //for sensible serialization with DataContractSerialier
    public class StubRequest
    {
        private static Random _random = new Random();

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }

        public static StubRequest GetRequest()
        {
            var req = new StubRequest();
            req.Id = _random.Next();
            req.Name = Guid.NewGuid().ToString();
            req.CreatedOn = DateTime.UtcNow.Date.AddDays(_random.Next(100, 1000) * -1);
            return req;
        }
    }
}
