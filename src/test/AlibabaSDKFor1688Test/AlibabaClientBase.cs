using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    public abstract class AlibabaClientBase
    {
        public AlibabaClientBase()
        {
            AlibabaClient = new AlibabaSDK.AlibabaStandardApiClient(ClientInfo.ClientId, ClientInfo.ClientSecret);
            AlibabaClient.SetAccessToken(ClientInfo.AccessToken);
        }
        public AlibabaSDK.AlibabaStandardApiClient AlibabaClient { get; }
    }
}
