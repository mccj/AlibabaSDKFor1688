using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    /// <summary>
    /// https://open.1688.com/solution/solutionDetail.htm?spm=0.$!pageSpm.0.0.589355edXjTGA8&solutionKey=1513248184893&category=SupplyCore#solutionDescribe
    /// </summary>
    public class 方案测试 : AlibabaClientBase
    {

        [Fact]
        public void Test1()
        {
            try
            {
                //获取系统时间
                var s0 = AlibabaClient.SystemTimeGet();
                //>获取交易订单的物流跟踪信息(买家视角)
                var s1 = AlibabaClient.AlibabaTradeGetLogisticsTraceInfoBuyerView(687707394323979339, "1688");
                //获取交易订单的物流信息(买家视角)
                var s2 = AlibabaClient.AlibabaTradeGetLogisticsInfosBuyerView(687707394323979339, "1688");
                //订单详情查看(买家视角)
                var s3 = AlibabaClient.AlibabaTradeGetBuyerView("1688", 687707394323979339);
                //订单列表查看(买家视角)
                var s4 = AlibabaClient.AlibabaTradeGetBuyerOrderList();
                //获取非授权用户的基本信息
                var s5 = AlibabaClient.AlibabaAccountAgentBasic("gamesalorcn");
                //失败消息批量确认
                var s6 = AlibabaClient.PushMessageConfirm(new[] { "5552111201" });
                //游标式获取失败的消息列表
                var s7 = AlibabaClient.PushCursorMessageList(quantity: 200);
                //查询式获取失败的消息列表
                var s8 = AlibabaClient.PushQueryMessageList(pageSize: 200);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
