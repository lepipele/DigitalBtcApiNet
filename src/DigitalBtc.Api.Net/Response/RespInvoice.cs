using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Response
{
    [DataContract]
    public class RespInvoice : RespBase
    {
        [DataMember(Name = "hash_val")]
        public string HashVal { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "time")]
        public string Time { get; set; }

        [DataMember(Name = "paid")]
        public bool Paid { get; set; }

        [DataMember(Name = "tx_id")]
        public string TxId { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "confirmations")]
        public int Confirmations { get; set; }

        [DataMember(Name = "amount_due")]
        public decimal AmountDue { get; set; }
    }
}