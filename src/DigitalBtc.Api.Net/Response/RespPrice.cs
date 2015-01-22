using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Response
{
    [DataContract]
    public class RespPrice : RespBase
    {
        [DataMember(Name = "side")]
        public string Side { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }
    }
}