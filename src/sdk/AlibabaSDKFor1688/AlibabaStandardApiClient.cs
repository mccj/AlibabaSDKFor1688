using AlibabaSDK.Utility;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AlibabaSDK
{
    public partial class AlibabaStandardApiClient : AlibabaApiClientBase
    {
        public AlibabaStandardApiClient(string appKey) : this(appKey, new HttpClient())
        {
        }
        public AlibabaStandardApiClient(string appKey, System.Net.Http.HttpClient httpClient) : this(httpClient)
        {
            base.setAppKey(appKey);
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
    }
}