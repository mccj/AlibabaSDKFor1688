using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    /// <summary>
    /// https://open.1688.com/solution/solutionDetail.htm?spm=0.$!pageSpm.0.0.589355edXjTGA8&solutionKey=1513248184893&category=SupplyCore#solutionDescribe
    /// </summary>
    public class �������� : AlibabaClientBase
    {

        [Fact]
        public void Test1()
        {
            try
            {
                //��ȡϵͳʱ��
                var s0 = AlibabaClient.SystemTimeGet();
                //>��ȡ���׶���������������Ϣ(����ӽ�)
                var s1 = AlibabaClient.AlibabaTradeGetLogisticsTraceInfoBuyerView(687707394323979339, "1688");
                //��ȡ���׶�����������Ϣ(����ӽ�)
                var s2 = AlibabaClient.AlibabaTradeGetLogisticsInfosBuyerView(687707394323979339, "1688");
                //��������鿴(����ӽ�)
                var s3 = AlibabaClient.AlibabaTradeGetBuyerView("1688", 687707394323979339);
                //�����б�鿴(����ӽ�)
                var s4 = AlibabaClient.AlibabaTradeGetBuyerOrderList();
                //��ȡ����Ȩ�û��Ļ�����Ϣ
                var s5 = AlibabaClient.AlibabaAccountAgentBasic("gamesalorcn");
                //ʧ����Ϣ����ȷ��
                var s6 = AlibabaClient.PushMessageConfirm(new[] { "5552111201" });
                //�α�ʽ��ȡʧ�ܵ���Ϣ�б�
                var s7 = AlibabaClient.PushCursorMessageList(quantity: 200);
                //��ѯʽ��ȡʧ�ܵ���Ϣ�б�
                var s8 = AlibabaClient.PushQueryMessageList(pageSize: 200);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
