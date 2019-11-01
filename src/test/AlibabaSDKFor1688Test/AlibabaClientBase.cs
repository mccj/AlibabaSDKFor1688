using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    public abstract class AlibabaClientBase
    {
        public AlibabaClientBase()
        {
            AlibabaClient = new AlibabaSDK.AlibabaClient(ClientInfo.ClientId);
            AlibabaClient.SetAccessToken(ClientInfo.AccessToken, ClientInfo.ClientSecret);
        }
        public AlibabaSDK.AlibabaClient AlibabaClient { get; }
    }
}
