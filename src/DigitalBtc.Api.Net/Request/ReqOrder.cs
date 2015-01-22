using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Request
{
    [DataContract]
    public class ReqOrder : ReqBase
    {
        public ReqOrder()
        {
            Method = "order";
            Side = "buy";
        }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "side")]
        public string Side { get; set; }
    }
}