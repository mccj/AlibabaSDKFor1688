using AlibabaSDK.Utility;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AlibabaSDK
{
    public partial class AlibabaFullApiClient : AlibabaApiClientBase
    {
        public AlibabaFullApiClient(string appKey, string clientSecret) : this(appKey, clientSecret, new HttpClient()) { }
        public AlibabaFullApiClient(string appKey, string clientSecret, HttpClient httpClient) : this(httpClient)
        {
            base.setAppKey(appKey, clientSecret);
        }
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            base.PrepareRequest(client, request, urlBuilder);
        }
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            base.PrepareRequest(client, request, url);
        }
        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            base.UpdateJsonSerializerSettings(settings);
        }
        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return base.ConvertToString(value, cultureInfo, this.JsonSerializerSettings);
        }
    }
}