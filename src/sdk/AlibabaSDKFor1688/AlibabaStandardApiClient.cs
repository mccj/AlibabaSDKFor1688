﻿using AlibabaSDK.Utility;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AlibabaSDK
{
    /// <summary>
    /// https://open.1688.com/api/apidoclist.htm?id=624392
    /// </summary>
    public partial class AlibabaStandardApiClient : AlibabaApiClientBase
    {
        public AlibabaStandardApiClient(string appKey, string clientSecret) : this(appKey, clientSecret, new HttpClient()) { }
        public AlibabaStandardApiClient(string appKey, string clientSecret, HttpClient httpClient) : this(httpClient)
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