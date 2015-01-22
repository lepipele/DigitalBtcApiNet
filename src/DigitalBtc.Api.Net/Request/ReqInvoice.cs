using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Request
{
    [DataContract]
    public class ReqInvoice : ReqBase
    {
        public ReqInvoice()
        {
            Method = "invoice";
        }
    }
}