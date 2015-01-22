using System;
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
            return ExecuteCall<RespOrder>(new ReqOrder {Price = price, Amount = amount, Address = address});
        }

        public RespInvoice Invoice()
        {
            return ExecuteCall<RespInvoice>(new ReqInvoice());
        }
    }

    // plumbing
    public partial class DigitalBtcApi
    {
        private const string baseUrl = "https://direct.digitalx.com";
        private readonly string _key;
        private readonly string _secret;

        public DigitalBtcApi(string key, string secret)
        {
            JsConfig.ExcludeTypeInfo = true;

            _key = key;
            _secret = secret;
        }

        private TResp ExecuteCall<TResp>(ReqBase req) where TResp : RespBase
        {
            req.JsonRequest = JsonSerializer.SerializeToString(req);

            string json = req.JsonRequest;

            //Encode the json version of the parameters above
            string convertedParam = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            //Encrypt the parameters with sha256 using the secret as the key
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(_secret);
            var hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(convertedParam);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            //Create the signature, a hex output of the encrypted hash above
            var stringBuilder = new StringBuilder();
            foreach (byte b in hashmessage)
            {
                stringBuilder.AppendFormat("{0:X2}", b);
            }
            string signature = stringBuilder.ToString().ToLower();


            String url = baseUrl + "/api/" + req.Method;
            WebRequest theRequest = WebRequest.Create(url);
            theRequest.Method = "POST";

            theRequest.ContentType = "text/x-json";
            theRequest.ContentLength = json.Length;
            theRequest.Headers["X-DIGITALX-KEY"] = _key;
            theRequest.Headers["X-DIGITALX-PARAMS"] = convertedParam;
            theRequest.Headers["X-DIGITALX-SIGNATURE"] = signature;
            Stream requestStream = theRequest.GetRequestStream();

            requestStream.Write(Encoding.ASCII.GetBytes(json), 0, json.Length);
            requestStream.Close();


            var response = (HttpWebResponse)theRequest.GetResponse();

            Stream responseStream = response.GetResponseStream();

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();

            response.Close();


            var obj = JsonSerializer.DeserializeFromString<TResp>(pageContent);
            obj.JsonResponse = pageContent;

            return obj;
        }
    }
}