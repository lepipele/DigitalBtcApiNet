using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Request
{
    [DataContract]
    public class ReqPrice : ReqBase
    {
        public ReqPrice()
        {
            Method = "price";
            Side = "buy";
        }

        [DataMember(Name = "side")]
        public string Side { get; set; }
    }
}