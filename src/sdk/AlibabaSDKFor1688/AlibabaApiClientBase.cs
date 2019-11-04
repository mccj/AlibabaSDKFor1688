using AlibabaSDK.Utility;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AlibabaSDK
{
    public partial class AlibabaApiClientBase
    {
        private string _appKey;
        private string _accessToken;
        private string _clientSecret;

        internal void setAppKey(string appKey)
        {
            this._appKey = appKey;
        }

        public void SetAccessToken(string accessToken, string clientSecret)
        {
            this._accessToken = accessToken;
            this._clientSecret = clientSecret;
        }
        internal void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            urlBuilder.Replace("{AppKey}", _appKey);
            if (request.Method == HttpMethod.Get)
            {
                var uri = new Uri(urlBuilder.ToString());
                var queryString = ParseQueryString(uri.Query);

                queryString["_aop_responseFormat"] = "json2";
                queryString["_aop_datePattern"] = "yyyyMMddHHmmssSSS";
                queryString["_aop_timestamp"] = DateTimeHelper.GetCurrentTimestamp().ToString();
                queryString["access_token"] = this._accessToken;

                //签名
                var urlPath = uri.AbsolutePath.Replace("/openapi/", "");
                var dictPara = queryString.AllKeys.ToDictionary(f => f, f => queryString.Get(f));
                var sign = SignatureHelper.HmacSha1(urlPath, dictPara, this._clientSecret);
                var signStr = SignatureHelper.ToHex(sign);
                queryString.Add("_aop_signature", signStr);

                var query = string.Join("&", queryString.AllKeys.Select(f => f + "=" + queryString.Get(f)));
                var newurl = new Uri(uri, "?" + query);
                urlBuilder.Clear();
                urlBuilder.Append(newurl.AbsoluteUri);
            }
        }
        internal void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {

            if (request.Method == HttpMethod.Post)
            {
                var uri = new Uri(url);
                var urlPath = uri.AbsolutePath.Replace("/openapi/", "");
                //var queryString = ParseQueryString(uri.Query);

                var content_ = request.Content as System.Net.Http.MultipartFormDataContent;
                var content2_ = request.Content as System.Net.Http.FormUrlEncodedContent;
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
                    //var queryString = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    //var dictParaQuery = queryString.AllKeys.ToDictionary(f => f, f => queryString.Get(f));
                    var dictPara = content_.ToDictionary(f => f.Headers.ContentDisposition.Name, f => f.ReadAsStringAsync().Result);
                    //var dictPara = dictParaQuery.Concat(dictParaForm).ToDictionary(f => f.Key, f => f.Value);
                    var sign = SignatureHelper.HmacSha1(urlPath, dictPara, this._clientSecret);
                    var signStr = SignatureHelper.ToHex(sign);
                    content_.Add(new System.Net.Http.StringContent(signStr), "_aop_signature");
                }
                else if (content2_ != null)
                {
                    var queryString = ParseQueryString(content2_.ReadAsStringAsync().Result);

                    var dictPara = queryString.AllKeys.ToDictionary(f => f, f => queryString.Get(f));

                    queryString["_aop_responseFormat"] = "json2";
                    queryString["_aop_datePattern"] = "yyyyMMddHHmmssSSS";
                    queryString["_aop_timestamp"] = DateTimeHelper.GetCurrentTimestamp().ToString();
                    queryString["access_token"] = this._accessToken;

                    var sign = SignatureHelper.HmacSha1(urlPath, dictPara, this._clientSecret);
                    var signStr = SignatureHelper.ToHex(sign);
                    dictPara.Add("_aop_signature", signStr);
                    request.Content = new System.Net.Http.FormUrlEncodedContent(dictPara);
                }
                else
                {
                    var dictPara = new System.Collections.Generic.Dictionary<string, string> {
                        //{ "_aop_responseFormat", "json2" },
                        //{ "_aop_datePattern", "yyyyMMddHHmmssSSS" },
                        //{ "_aop_timestamp", DateTimeHelper.GetCurrentTimestamp().ToString() },
                        { "access_token", this._accessToken }
                    };
                    var sign = SignatureHelper.HmacSha1(urlPath, dictPara, this._clientSecret);
                    var signStr = SignatureHelper.ToHex(sign);
                    dictPara.Add("_aop_signature", signStr);
                    request.Content = new System.Net.Http.FormUrlEncodedContent(dictPara);


                    var ssss = new System.Net.Http.FormUrlEncodedContent(dictPara).ReadAsStringAsync().Result;
                }
            }
        }
        internal void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            //"yyyyMMddHHmmssfff"
            settings.DateFormatString = "yyyyMMddHHmmssfffzzz";
        }
        private System.Collections.Specialized.NameValueCollection ParseQueryString(string query)
        {
            var nv = new System.Collections.Specialized.NameValueCollection();
            foreach (var item in query?.Trim('?')?.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { })
            {
                var n = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                nv[n.FirstOrDefault()] = n.Skip(1).FirstOrDefault();
            }
            return nv;
        }
    }
}