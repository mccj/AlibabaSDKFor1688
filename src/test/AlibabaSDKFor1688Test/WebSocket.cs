using AlibabaSDK;
using System;
using Xunit;

namespace AlibabaSDKFor1688Test
{
    /// <summary>
    /// https://open.1688.com/solution/solutionDetail.htm?spm=0.$!pageSpm.0.0.589355edXjTGA8&solutionKey=1513248184893&category=SupplyCore#solutionDescribe
    /// </summary>
    public class WebSocket111
    {

        [Fact(Skip = "跳过")]
        public void Test1()
        {
            //try
            //{
            //    var content = "{\"bizKey\":\"152539133487786140\",\"data\":{\"supplierMemberId\":\"b2b-3081850159d0afc\",\"bizType\":\"mall\",\"orderId\":152539133487786140,\"Items\":[{\"productCode\":\"MMA00152\",\"productId\":2227112,\"orderItemId\":152539133487786140,\"productQuoteId\":569969887292}],\"subUserId\":2999689623},\"gmtBorn\":1572867144980,\"msgId\":5528020370,\"type\":\"CAIGOU_MSG_BUYER_ORDERED\",\"userInfo\":\"qdi2oh0A4dse\"}";
            //    var ssss = Newtonsoft.Json.JsonConvert.DeserializeObject<AlibabaSDK.WebSocketModels.ReceivedMessageData<object>>(content);

            //}
            //catch (Exception ex)
            //{

            //}


            var ddddd = new AlibabaWebSocketClient(ClientInfo.ClientId, ClientInfo.ClientSecret)
            {
                ReceivedMessageFunc = (msgSource, data) =>
                {
                    System.Diagnostics.Debug.WriteLine((msgSource == MsgSource.MOCK ? "测试数据" : "生产数据") + "   " + data.Type + "   " + data.TypeDescription + "   " + data.UserInfo + "   " + data.ToJson());
                    return true;
                },
                SystemInfoFunc = (msgSource, msg) =>
                {
                    System.Diagnostics.Debug.WriteLine((msgSource == MsgSource.MOCK ? "测试数据" : "生产数据") + "    系统消息:" + msg);
                },
                Loglogger = (logType, log, ex) =>
                {
                    System.Diagnostics.Debug.WriteLine("日志信息:" + log + ";   " + ex?.Message);
                }
            };
            ddddd.Connect();


            //System.Net.WebSockets.ClientWebSocket clientWebSocket = new System.Net.WebSockets.ClientWebSocket();
            //await clientWebSocket.ConnectAsync(new Uri("ws://message.1688.com/websocket"), System.Threading.CancellationToken.None);
            //var rr = System.Threading.Tasks.Task.Run(async () =>
            //        {
            //            while (true)
            //            {
            //                var buffer = new ArraySegment<byte>(new byte[1024]);
            //                var wsdata = await clientWebSocket.ReceiveAsync(buffer, System.Threading.CancellationToken.None);

            //                var str = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
            //            }
            //        });

            //var d = new WebSocketMessage()
            //{
            //    appKey = ClientInfo.ClientId,
            //    type = WebSocketMessageType.CONNECT.ToString(),
            //    pubTime = AlibabaSDK.Utility.DateTimeHelper.GetCurrentTimestamp(),
            //};
            //d.sign = sign(d, ClientInfo.ClientSecret);
            //var vvv = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            //await clientWebSocket.SendAsync(System.Text.Encoding.UTF8.GetBytes(vvv), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
            System.Threading.Thread.Sleep(1000 * 10 * 10 * 100 * 100);
        }

        [Fact]
        public void dddd()
        {
            var dddd = AlibabaWebSocketClient.GetReceivedMessageData("CAIGOU_MSG_BUYER_PUBLISH_BUYOFFER", "{data:{}}");
            dynamic ee = dddd;
            var sss = ee.Data as AlibabaSDK.WebSocketModels.CAIGOU_MSG_BUYER_PUBLISH_BUYOFFER;
            Assert.NotNull(sss);
        }
        [Fact]
        public void dddd1()
        {
            var dddd = AlibabaWebSocketClient.GetReceivedMessageData("PRODUCT_PRODUCT_INVENTORY_CHANGE", "{\"msgId\":5746327830,\"type\":\"PRODUCT_PRODUCT_INVENTORY_CHANGE\",\"userInfo\":\"\",\"data\":{\"OfferInventoryChangeList\":[{\"offerOnSale\":225936008,\"quantity\":-2,\"offerId\":42872301173,\"skuOnSale\":56999118,\"skuId\":3881120556878},{\"offerOnSale\":225936008,\"quantity\":-1,\"offerId\":42872301173,\"skuOnSale\":502732,\"skuId\":78376823121},{\"offerOnSale\":225936008,\"quantity\":-2,\"offerId\":42872301173,\"skuOnSale\":56210321,\"skuId\":78376823124},{\"offerOnSale\":225936008,\"quantity\":-4,\"offerId\":42872301173,\"skuOnSale\":504936,\"skuId\":78376823119},{\"offerOnSale\":225936008,\"quantity\":-1,\"offerId\":42872301173,\"skuOnSale\":5296386,\"skuId\":3881120556880},{\"offerOnSale\":225936008,\"quantity\":-1,\"offerId\":42872301173,\"skuOnSale\":526494,\"skuId\":78376823123}]},\"gmtBorn\":1574055094000}");
            dynamic ee = dddd;
            var sss = ee.Data as AlibabaSDK.WebSocketModels.PRODUCT_PRODUCT_INVENTORY_CHANGE;
            Assert.NotNull(sss);
        }
    }

}
