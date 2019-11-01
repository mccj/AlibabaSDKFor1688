using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    public class AlibabaClientTest
    {
        [Fact]
        public void Test1()
        {
            var alibabaClient = new AlibabaSDK.AlibabaClient(ClientInfo.ClientId);
            alibabaClient.SetAccessToken(ClientInfo.AccessToken, ClientInfo.ClientSecret);

            var ss = alibabaClient.AlibabaAeMessagePushPerfData();
            var rr = alibabaClient.AlibabaAccountAgentBasic("gamesalorcn", "");
            //loginId=gamesalorcn&access_token=4ceb6921-d2fd-4cac-8eb0-92befd9be410&_aop_signature=09CA3BAB7B7D608C1EF10EDE847628DBA941EBC6
            //loginId=gamesalorcn&access_token=4ceb6921-d2fd-4cac-8eb0-92befd9be410&_aop_timestamp=1572426027001&_aop_signature=6A4D4C2DF7A721BE2CEE1EC3AFFEB6D279E7A853
            //loginId=gamesalorcn&access_token=4ceb6921-d2fd-4cac-8eb0-92befd9be410&aaaaaa=bbbbbb&_aop_timestamp=1572426061668&_aop_signature=3727403F61F3394DCB38F560A4D288F850D8D2B9
            //access_token=4ceb6921-d2fd-4cac-8eb0-92befd9be410&loginId=gamesalorcn&_aop_timestamp=1572426097190&aaaaaa=bbbbbb&_aop_signature=9901A83A53EEDDC4C5BC21BF36382CC02940DFD8

        }
    }
}
