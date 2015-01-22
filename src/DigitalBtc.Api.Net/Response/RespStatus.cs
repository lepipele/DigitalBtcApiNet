using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Response
{
    [DataContract]
    public class RespStatus : RespBase
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}