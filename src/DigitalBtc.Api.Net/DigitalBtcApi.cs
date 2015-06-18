using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using DigitalBtc.Api.Net.Request;
using DigitalBtc.Api.Net.Response;
using ServiceStack.Text;

namespace DigitalBtc.Api.Net
{
    // calls
    public partial class DigitalBtcApi
    {
        public RespPrice Price()
        {
            return ExecuteCall<RespPrice>(new ReqPrice());
        }

        public RespStatus Status()
        {
            return ExecuteCall<RespStatus>(new ReqStatus());
        }

        public RespOrder Order(decimal price, decimal amount, string address = null)
        {
            if (_debugDummyOrders)
            {
                var resp = new RespOrder
                {
                    Price = price,
                    Amount = amount,
                    TxId = "00000000-0000-0000-0000-000000000000",
                    PayoutAddress = address,
                    Message = "DEBUG_DUMMY_ORDER",
                    Success = 1
                };
                return resp;
            }
            else
            {
                return ExecuteCall<RespOrder>(new ReqOrder { Price = price, Amount = amount, Address = address });
            }
        }

        public RespInvoice Invoice()
        {
            return ExecuteCall<RespInvoice>(new ReqInvoice());
        }
    }

    // flags
    public partial class DigitalBtcApi
    {
        private readonly bool _debugDummyOrders;

        // TODO: Remove from library after testing
        private readonly string _debugSuffix;
    }

    // plumbing
    public partial class DigitalBtcApi
    {
        private static DigitalBtcApi _instance = new DigitalBtcApi(
            DigitalBtcSettings.Default.Key, DigitalBtcSettings.Default.Secret,
            DigitalBtcSettings.Default.DebugDummyOrders, DigitalBtcSettings.Default.DebugSuffix);

        public static DigitalBtcApi Instance
        {
            get { return _instance; }
        }

        private const string baseUrl = "https://api.direct.digitalx.com/v0";
        private readonly string _key;
        private readonly string _secret;

        public DigitalBtcApi(string key, string secret, bool debugDummyOrders, string debugSuffix)
        {
            JsConfig.ExcludeTypeInfo = true;

            _key = key;
            _secret = secret;

            _debugDummyOrders = debugDummyOrders;
            _debugSuffix = debugSuffix;
        }

        private TResp ExecuteCall<TResp>(ReqBase req) where TResp : RespBase
        {
            req.JsonRequest = JsonSerializer.SerializeToString(req);

            string json = req.JsonRequest;

            //Encode the json version of the parameters above
            string convertedParam = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            // Encrypt the parameters with sha256 using the secret as the key
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(_secret);
            var hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(convertedParam);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            // Create the signature, a hex output of the encrypted hash above
            var stringBuilder = new StringBuilder();
            foreach (byte b in hashmessage)
            {
                stringBuilder.AppendFormat("{0:X2}", b);
            }
            string signature = stringBuilder.ToString().ToLower();


            String url = baseUrl + "/" + req.Method + _debugSuffix;
            WebRequest theRequest = WebRequest.Create(url);
            theRequest.Method = "POST";

            theRequest.ContentType = "text/x-json";
            theRequest.ContentLength = json.Length;
            theRequest.Headers["X-DIGITALX-KEY"] = _key;
            theRequest.Headers["X-DIGITALX-PARAMS"] = convertedParam;
            theRequest.Headers["X-DIGITALX-SIGNATURE"] = signature;
            using (Stream requestStream = theRequest.GetRequestStream())
            {
                requestStream.Write(Encoding.ASCII.GetBytes(json), 0, json.Length);
                requestStream.Close();

                using (var response = (HttpWebResponse)theRequest.GetResponse())
                using (var responseStream = response.GetResponseStream())
                // TODO: responseStream can be null
                using (var myStreamReader = new StreamReader(responseStream, Encoding.Default))
                {
                    string pageContent = myStreamReader.ReadToEnd();

                    var obj = JsonSerializer.DeserializeFromString<TResp>(pageContent);
                    obj.JsonResponse = pageContent;

                    return obj;
                }
            }
        }
    }
}