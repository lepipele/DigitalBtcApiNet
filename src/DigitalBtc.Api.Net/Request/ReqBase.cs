using System;
using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Request
{
    [DataContract]
    public abstract class ReqBase
    {
        protected ReqBase()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            Nonce = t.Ticks;
        }

        [DataMember(Name = "method")]
        public string Method { get; set; }

        [DataMember(Name = "nonce")]
        public long Nonce { get; set; }

        [IgnoreDataMember]
        public string JsonRequest { get; set; }
    }
}