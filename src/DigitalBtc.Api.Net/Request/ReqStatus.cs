using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Request
{
    [DataContract]
    public class ReqStatus : ReqBase
    {
        public ReqStatus()
        {
            Method = "status";
        }
    }
}