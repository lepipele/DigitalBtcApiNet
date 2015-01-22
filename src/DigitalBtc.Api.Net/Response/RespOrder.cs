using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Response
{
    [DataContract]
    public class RespOrder : RespBase
    {
        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "tx_id")]
        public string TxId { get; set; }

        [DataMember(Name = "payout_address")]
        public string PayoutAddress { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}