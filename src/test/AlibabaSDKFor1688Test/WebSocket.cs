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

        [Fact]
        public  void Test1()
        {
            //try
            //{
            //    var content = "{\"bizKey\":\"152539133487786140\",\"data\":{\"supplierMemberId\":\"b2b-3081850159d0afc\",\"bizType\":\"mall\",\"orderId\":152539133487786140,\"Items\":[{\"productCode\":\"MMA00152\",\"productId\":2227112,\"orderItemId\":152539133487786140,\"productQuoteId\":569969887292}],\"subUserId\":2999689623},\"gmtBorn\":1572867144980,\"msgId\":5528020370,\"type\":\"CAIGOU_MSG_BUYER_ORDERED\",\"userInfo\":\"qdi2oh0A4dse\"}";
            //    var ssss = Newtonsoft.Json.JsonConvert.DeserializeObject<AlibabaSDK.WebSocketModels.ReceivedMessageData<object>>(content);

            //}
            //catch (Exception ex)
            //{

            //}


            var ddddd = new AlibabaWebSocketClient(ClientInfo.ClientId, ClientInfo.ClientSecret, (d, f) =>
            {
                System.Diagnostics.Debug.WriteLine((d == MsgSource.MOCK ? "测试数据" : "生产数据") + "   " + f.Type + "   " + f.TypeDescription + "   " + f.UserInfo+"   "+f.ToJson());
                return true;
            });
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

        //[Fact]
        //public void dddd()
        //{
        //    var sss = new AlibabaWebSocketClient("", "", null);
        //    var dddd = sss.GetReceivedMessageData("CAIGOU_MSG_MALL_GOODS", "{}");
        //}
        //private string sign(WebSocketMessage tm, string secret)
        //{
        //    var query = new System.Text.StringBuilder(secret);
        //    query.Append(tm.appKey);
        //    //query.Append(tm.content);
        //    query.Append(tm.pubTime);
        //    byte[] bytes = encryptMD5(query.ToString());

        //    return AlibabaSDK.Utility.SignatureHelper.ToHex(bytes);
        //}

        //private byte[] encryptMD5(string dd)
        //{
        //    var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    var r = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dd));
        //    return r;
        //}
    }

    //public class WebSocketMessage
    //{
    //    //      /**
    //    //* 系统内部字段
    //    //*/
    //    //      private long relatedId;

    //    //      private long relatedMsgTime;

    //    /**
    //     * 应用 AppKey
    //     */
    //    public String appKey { get; set; }

    //    /**
    //     * 应用 AppSecret
    //     */
    //    String secret;

    //    /**
    //     * webocket消息类型
    //     */
    //    public string type { get; set; }

    //    ///**
    //    // * 消息id
    //    // */
    //    //private String id;

    //    ///**
    //    // * 消息推送时间
    //    // */
    //    public long pubTime { get; set; }

    //    ///**
    //    // * 消息内容，json串格式
    //    // */
    //    //private String content;

    //    ///**
    //    // * 签名
    //    // */
    //    public String sign { get; set; }

    //    ///**
    //    // * isv端消息处理耗时
    //    // */
    //    //private long costInIsv;

    //    ///**
    //    // * 数据来源。包括
    //    // * MOCK: 测试数据。使用消息测试工具产生。
    //    // * REAL: 真实数据。此字段为空时默认为真实数据。
    //    // */
    //    //private String msgSource;
    //}

    //public enum WebSocketMessageType
    //{
    //    CONNECT, CONNECT_ACK, HEARTBEAT, CONFIRM, SERVER_PUSH, CLIENT_PUSH, CLOSE, SYSTEM
    //}
}
