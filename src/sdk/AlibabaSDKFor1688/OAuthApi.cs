using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AlibabaSDK
{
    using AlibabaSDK.Utility;
    using Models;
    /// <summary>
    /// 授权、令牌相关 Api
    /// </summary>
    public static class OAuthApi
    {
        private static HttpClient _httpClient = new HttpClient();
        #region 同步请求
        /// <summary>
        /// 获取授权请求地址
        /// 
        /// 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
        /// 如果用户同意授权，页面将跳转至 redirect_uri?code={authorization_code}。这里的code用于换取access_token 
        /// 若用户禁止授权，则重定向后不会带上code参数 
        /// </summary>
        /// <param name="clientId">app注册时，分配给app的唯一标示，又称appKey</param>
        /// <param name="site">site参数标识当前授权的站点，直接填写1688</param>
        /// <param name="redirectUrl">
        /// app的入口地址，授权临时令牌会以queryString的形式跟在该url后返回。注意参数中回调地址的域名必须与app注册时填写的回调地址的域名匹配。
        /// 
        /// redirect_uri有三种方式，每种方式对应的code返回形式不同，isv可根据自己的情况选择
        ///     i. urn:ietf:wg:oauth:2.0:oob
        /// Code以body的方式返回到默认的alibaba页面
        ///     ii. http://localhost:port
        /// Code以queryString的方式返回到url，app需监听本地端口，接收code(windows系统会受到防火墙的干扰)
        ///     iii. https://auth.1688.com/auth/authCode.htm
        /// Code以queryString的方式返回到该url
        /// </param>
        /// <param name="state">可选，app自定义参数，回跳到redirect_uri时，会原样返回</param>
        /// <returns>请求地址</returns>
        public static string GetAuthorizeUrl(
            string clientId,
            string redirectUrl = "https://auth.1688.com/auth/authCode.htm",
            string site = "1688",
            string state = null)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(redirectUrl)) throw new ArgumentNullException(nameof(redirectUrl));
            if (string.IsNullOrWhiteSpace(site)) throw new ArgumentNullException(nameof(site));

            var url = "https:" + $"//auth.1688.com/oauth/authorize?client_id={clientId.AsUrlData()}&site={site.AsUrlData()}&redirect_uri={redirectUrl.AsUrlData()}&state={state.AsUrlData()}";

            return url;
        }

        /// <summary>
        /// 使用code获取令牌
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="preAuthCode">临时令牌 code</param>
        /// <param name="redirectUrl">回调地址</param>
        /// <param name="needRefreshToken">是否需要刷新token</param>
        /// <returns></returns>
        public static OAuthAccessTokenResult GetAccessToken(
            string clientId,
            string clientSecret,
            string preAuthCode,
            string redirectUrl = "default",
            bool needRefreshToken = true)
        {
            return Task.Run(async () =>
                 await GetAccessTokenAsync(clientId, clientSecret, preAuthCode, redirectUrl, needRefreshToken, CancellationToken.None)
            ).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 换取accessToken
        /// 
        /// 注意：1688并未在结果中包含refresh_token,所以不要用返回数据中的refresh_token覆盖
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="refreshToken">刷新码</param>
        /// <returns></returns>
        public static OAuthAccessTokenResult RefreshAccessToken(
            string clientId,
            string clientSecret,
            string refreshToken)
        {
            return Task.Run(async () =>
                 await RefreshAccessTokenAsync(clientId, clientSecret, refreshToken, CancellationToken.None)
            ).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 换取新的refreshToken
        /// 
        /// 如果当前时间离refreshToken过期时间在30天以内，那么可以调用postponeToken接口换取新的refreshToken；否则会报错
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="refreshToken">刷新码</param>
        /// <param name="accessToken">访问码</param>
        /// <returns></returns>
        public static OAuthAccessTokenResult RenewRefreshToken(
            string clientId,
            string clientSecret,
            string accessToken,
            string refreshToken)
        {
            return Task.Run(async () =>
                await RenewRefreshTokenAsync(clientId, clientSecret, accessToken, refreshToken, CancellationToken.None)
            ).GetAwaiter().GetResult();
        }

        #endregion
        #region 异步请求
        /// <summary>
        /// 使用code获取令牌
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="preAuthCode">临时令牌 code</param>
        /// <param name="redirectUrl">回调地址</param>
        /// <param name="needRefreshToken">是否需要刷新token</param>
        /// <returns></returns>
        public static async Task<OAuthAccessTokenResult> GetAccessTokenAsync(
            string clientId,
            string clientSecret,
            string preAuthCode,
            string redirectUrl = "default",
            bool needRefreshToken = true)
        {
            return await GetAccessTokenAsync(clientId, clientSecret, preAuthCode, redirectUrl, needRefreshToken);
        }

        /// <summary>
        /// 使用code获取令牌
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="preAuthCode">临时令牌 code</param>
        /// <param name="redirectUrl">回调地址</param>
        /// <param name="needRefreshToken">是否需要刷新token</param>
        /// <returns></returns>
        public static async Task<OAuthAccessTokenResult> GetAccessTokenAsync(
        string clientId,
        string clientSecret,
        string preAuthCode,
        string redirectUrl,
        bool needRefreshToken,
        CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

            var url = $"https:" + $"//gw.open.1688.com/openapi/http/1/system.oauth2/getToken/{clientId.AsUrlData()}";
            var response = await _httpClient.PostAsync(url,
                new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "grant_type","authorization_code"},
                    { "need_refresh_token",needRefreshToken.ToString()},
                    { "client_id",clientId},
                    { "client_secret",clientSecret},
                    { "redirect_uri",redirectUrl},
                    { "code",preAuthCode}
                }), cancellationToken
            );
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(json);
            //var result = await response.Content.ReadAsAsync<OAuthAccessTokenResult>(cancellationToken);
            return result;
        }
        /// <summary>
        /// 换取accessToken
        /// 
        /// 注意：1688并未在结果中包含refresh_token,所以不要用返回数据中的refresh_token覆盖
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="refreshToken">刷新码</param>
        /// <returns></returns>
        public async static Task<OAuthAccessTokenResult> RefreshAccessTokenAsync(
            string clientId,
            string clientSecret,
            string refreshToken)
        {
            return await RefreshAccessTokenAsync(clientId, clientSecret, refreshToken, CancellationToken.None);
        }
        /// <summary>
        /// 换取accessToken
        /// 
        /// 注意：1688并未在结果中包含refresh_token,所以不要用返回数据中的refresh_token覆盖
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="refreshToken">刷新码</param>
        /// <returns></returns>
        public async static Task<OAuthAccessTokenResult> RefreshAccessTokenAsync(
        string clientId,
        string clientSecret,
        string refreshToken,
        CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

            var url = $"https://gw.open.1688.com/openapi/param2/1/system.oauth2/getToken/{clientId.AsUrlData()}";
            var response = await _httpClient.PostAsync(url,
                new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "grant_type","refresh_token"},
                    { "client_id",clientId},
                    { "client_secret",clientSecret},
                    { "refresh_token",refreshToken}
                }), cancellationToken
            );
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(json);
            //var result = await response.Content.ReadAsAsync<OAuthAccessTokenResult>(cancellationToken);
            return result;
        }
        /// <summary>
        /// 换取新的refreshToken
        /// 
        /// 如果当前时间离refreshToken过期时间在30天以内，那么可以调用postponeToken接口换取新的refreshToken；否则会报错
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="refreshToken">刷新码</param>
        /// <param name="accessToken">访问码</param>
        /// <returns></returns>
        public async static Task<OAuthAccessTokenResult> RenewRefreshTokenAsync(
            string clientId,
            string clientSecret,
            string accessToken,
            string refreshToken)
        {
            return await RenewRefreshTokenAsync(clientId, clientSecret, accessToken, refreshToken, CancellationToken.None);
        }
        /// <summary>
        /// 换取新的refreshToken
        /// 
        /// 如果当前时间离refreshToken过期时间在30天以内，那么可以调用postponeToken接口换取新的refreshToken；否则会报错
        /// </summary>
        /// <param name="clientId">appKey</param>
        /// <param name="clientSecret">app密钥</param>
        /// <param name="refreshToken">刷新码</param>
        /// <param name="accessToken">访问码</param>
        /// <returns></returns>
        public async static Task<OAuthAccessTokenResult> RenewRefreshTokenAsync(
            string clientId,
            string clientSecret,
            string accessToken,
            string refreshToken,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrWhiteSpace(accessToken)) throw new ArgumentNullException(nameof(accessToken));
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

            var url = $"https://gw.open.1688.com/openapi/param2/1/system.oauth2/postponeToken/{clientId.AsUrlData()}";
            var response = await _httpClient.PostAsync(url,
                new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "client_id",clientId},
                    { "client_secret",clientSecret},
                    { "refresh_token",refreshToken},
                    { "access_token",accessToken}
                }), cancellationToken
            );
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OAuthAccessTokenResult>(json);
            //var result = await response.Content.ReadAsAsync<OAuthAccessTokenResult>(cancellationToken);
            return result;
        }
        #endregion
    }

    namespace Models
    {

        ///// <summary>
        ///// 所有JSON格式返回值的API返回结果接口
        ///// </summary>
        //public interface IJsonResult
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    string message { get; set; }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    int code { get; set; }

        //    /// <summary>
        //    /// 错误描述
        //    /// </summary>
        //    string error { get; set; }


        //    /// <summary>
        //    /// 错误码
        //    /// </summary>
        //    string errorCode { get; set; }

        //    /// <summary>
        //    /// 错误信息
        //    /// </summary>
        //    string errorMessage { get; set; }

        //    #region webexception

        //    /// <summary>
        //    /// 请求标识
        //    /// </summary>
        //    string request_id { get; set; }

        //    /// <summary>
        //    /// 错误描述
        //    /// </summary>
        //    string error_description { get; set; }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    string error_code { get; set; }

        //    /// <summary>
        //    /// 异常描述 
        //    /// </summary>
        //    string exception { get; set; }

        //    #endregion
        //}

        /// <summary>
        /// JSON返回结果
        /// </summary>
        //[Serializable]
        public class AliJsonResult// : IJsonResult
        {

            private string _code;

            private string _error;

            ///// <summary>
            ///// 
            ///// </summary>
            //public string message { get; set; }

            ///// <summary>
            ///// 
            ///// </summary>
            //public int code
            //{
            //    get
            //    {
            //        int c = -1;
            //        int.TryParse(_code, out c);
            //        return c;
            //    }
            //    set { _code = Convert.ToString(value); }
            //}

            ////public virtual object data { get; set; }

            /// <summary>
            /// 错误代码或是简述
            /// </summary>
            [JsonProperty("error")]
            public string error
            {
                get { return _error; }
                set { _error = value; }
            }


            ///// <summary>
            ///// 错误码
            ///// </summary>
            //public string errorCode
            //{
            //    get { return _code; }
            //    set { _code = value; }
            //}

            /// <summary>
            /// 错误信息
            /// </summary>
            [JsonProperty("error_message")]
            public string ErrorMessage
            {
                get { return _error; }
                set { _error = value; }
            }

            //#region webexception

            /// <summary>
            /// 请求标识
            /// </summary>
            [JsonProperty("request_id")]
            public string RequestId { get; set; }

            /// <summary>
            /// 错误描述
            /// </summary>
            [JsonProperty("error_description")]
            public string ErrorDescription { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("error_code")]
            public string ErrorCode
            {
                get { return _code; }
                set { _code = value; }
            }

            /// <summary>
            /// 异常描述 
            /// </summary>
            [JsonProperty("exception")]
            public string Exception { get; set; }

            //#endregion

            ///// <summary>
            ///// 
            ///// </summary>
            ///// <returns></returns>
            //public override string ToString()
            //{
            //    return string.Format("AliJsonResult:{{message:'{0}',code:'{1}', errorCode:'{2}',errorMessage:'{3}'}}", message, code, errorCode, errorMessage);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public class OAuthAccessTokenResult : AliJsonResult
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("aliId")]
            public string AliId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("memberId")]
            public string MemberId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("resource_owner")]
            public string Resource_owner { get; set; }

            /// <summary>
            /// 访问令牌
            /// 有效期 10 小时
            /// </summary>
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            /// <summary>
            /// 刷新令牌
            /// 有效期半年
            /// </summary>
            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

            /// <summary>
            /// 令牌有效期（秒）
            /// </summary>
            [JsonProperty("expires_in")]
            public int? ExpiresIn
            {
                get => _expiresIn;
                set
                {
                    if (value.HasValue)
                    {
                        AccessTokenTimeoutEx = System.DateTimeOffset.Now.AddSeconds(value.Value);
                        ExpiresInEx = TimeSpan.FromSeconds(value.Value);
                    }
                    else
                    {
                        ExpiresInEx = null;
                        AccessTokenTimeoutEx = null;
                    }
                    _expiresIn = value;
                }
            }
            private int? _expiresIn;
            /// <summary>
            /// 刷新令牌的有效期
            /// </summary>
            [JsonProperty("refresh_token_timeout")]
            public string RefreshTokenTimeout { get; set; }

            /// <summary>
            /// 刷新令牌的有效期
            /// </summary>
            public DateTimeOffset? RefreshTokenTimeoutEx
            {
                get
                {
                    if (!string.IsNullOrEmpty(RefreshTokenTimeout))
                    {
                        try
                        {
                            return DateTimeHelper.FormatFromStr(RefreshTokenTimeout);
                        }
                        catch// (Exception)
                        {

                        }
                    }

                    return null;
                }
            }
            public TimeSpan? ExpiresInEx { get; private set; }
            public DateTimeOffset? AccessTokenTimeoutEx { get; private set; }
        }
    }
}