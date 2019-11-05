using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    public abstract class AlibabaClientBase
    {
        public AlibabaClientBase()
        {
            AlibabaClient = new AlibabaSDK.AlibabaFullApiClient(ClientInfo.ClientId, ClientInfo.ClientSecret);
            AlibabaClient.SetAccessToken(ClientInfo.AccessToken);
        }
        public AlibabaSDK.AlibabaFullApiClient AlibabaClient { get; }
    }
}
