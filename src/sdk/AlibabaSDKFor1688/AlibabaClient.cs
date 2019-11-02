using AlibabaSDK.Utility;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AlibabaSDK
{
    public partial class AlibabaClient
    {
        private readonly string _appKey;
        private string _accessToken;
        private string _clientSecret;

        public AlibabaClient(string appKey) : this(appKey, new HttpClient())
        {
        }
        public AlibabaClient(string appKey, System.Net.Http.HttpClient httpClient) : this(httpClient)
        {
            this._appKey = appKey;
        }

        public void SetAccessToken(string accessToken, string clientSecret)
        {
            this._accessToken = accessToken;
            this._clientSecret = clientSecret;
        }
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            urlBuilder.Replace("{AppKey}", _appKey);
        }
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            var content_ = request.Content as System.Net.Http.MultipartFormDataContent;
            if (content_ != null)
            {
                ////返回的数据格式(可选)
                //content_.Add(new System.Net.Http.StringContent("json2"), "_aop_responseFormat");//json2,xml2,xml,json
                ////时间格式(可选)
                //content_.Add(new System.Net.Http.StringContent("yyyyMMddHHmmssSSS"), "_aop_datePattern");
                ////时间戳(可选)
                //content_.Add(new System.Net.Http.StringContent(DateTimeHelper.GetCurrentTimestamp().ToString()), "_aop_timestamp");
                //授权码
                content_.Add(new System.Net.Http.StringContent(this._accessToken), "access_token");

                //签名
                var uri = new Uri(url);
                var urlPath = uri.AbsolutePath.Replace("/openapi/", "");
                //var queryString = System.Web.HttpUtility.ParseQueryString(uri.Query);
                //var dictParaQuery = queryString.AllKeys.ToDictionary(f => f, f => queryString.Get(f));
                var dictPara = content_.ToDictionary(f => f.Headers.ContentDisposition.Name, f => f.ReadAsStringAsync().Result);
                //var dictPara = dictParaQuery.Concat(dictParaForm).ToDictionary(f => f.Key, f => f.Value);
                var sign = SignatureHelper.HmacSha1(urlPath, dictPara, this._clientSecret);
                var signStr = SignatureHelper.ToHex(sign);
                content_.Add(new System.Net.Http.StringContent(signStr), "_aop_signature");
            }//                //System.Web.HttpUtility.ParseQueryString("");
        }
    }
}