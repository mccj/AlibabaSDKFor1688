using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    /// <summary>
    /// https://open.1688.com/solution/solutionDetail.htm?spm=0.$!pageSpm.0.0.589355edXjTGA8&solutionKey=1513248184893&category=SupplyCore#solutionDescribe
    /// </summary>
    public class ∑Ω∞∏≤‚ ‘ : AlibabaClientBase
    {

        [Fact]
        public void Test1()
        {
            try
            {
                var s0 = AlibabaClient.SystemTimeGet();
                var s1 = AlibabaClient.AlibabaTradeGetLogisticsTraceInfoBuyerView(687707394323979339, "1688");
                var s2 = AlibabaClient.AlibabaTradeGetLogisticsInfosBuyerView(687707394323979339, "1688");

                var s3 = AlibabaClient.AlibabaTradeGetBuyerView("1688", 687707394323979339);
                var s4 = AlibabaClient.AlibabaTradeGetBuyerOrderList();

                var s6 = AlibabaClient.AlibabaAccountAgentBasic("gamesalorcn");
                //var s5 = AlibabaClient.AlibabaAccountBasic();

            }
            catch (Exception ex)
            {
            }
        }
    }
}
