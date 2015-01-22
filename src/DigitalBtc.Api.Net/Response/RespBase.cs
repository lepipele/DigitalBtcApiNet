using System.Runtime.Serialization;

namespace DigitalBtc.Api.Net.Response
{
    [DataContract]
    public class RespBase
    {
        [DataMember(Name = "success")]
        public int Success { get; set; }

        [DataMember(Name = "errors")]
        public string Errors { get; set; }


        [IgnoreDataMember]
        public bool IsSuccess { get { return Success == 1; }}
        [IgnoreDataMember]
        public string JsonResponse { get; set; }
    }
}