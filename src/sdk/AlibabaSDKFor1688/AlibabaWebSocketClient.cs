using AlibabaSDK.Utility;
using AlibabaSDK.WebSocketModels;
using Newtonsoft.Json;
using System;

namespace AlibabaSDK
{
    public partial class AlibabaWebSocketClient
    {
        private readonly string _appKey;
        private string _clientSecret;
        private System.Net.WebSockets.ClientWebSocket clientWebSocket = null;
        private JsonSerializerSettings serializerSettings;

        public AlibabaWebSocketClient(string appKey, string clientSecret)
        {
            serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            this._appKey = appKey;
            this._clientSecret = clientSecret;
        }
        public Func<MsgSource, ReceivedMessageDataBase, bool> ReceivedMessageFunc { get; set; }
        public Action<MsgSource, string> SystemInfoFunc { get; set; }
        public Action<LogType, string, Exception> Loglogger { get; set; }
        public Action<AlibabaWebSocketClient, bool> ConnectStateChange { get; set; }
        private void Log(LogType logType, string log, Exception ex = null)
        {
            if (Loglogger != null)
            {
                try
                {
                    Loglogger(logType, log, ex);
                }
                catch (Exception logex)
                {
                }
            }
            else
                System.Diagnostics.Debug.WriteLine(logType + " " + log + " " + ex.Message);
        }

        private bool ____isConnect = false;
        public bool IsConnect
        {
            get { return this.____isConnect; }
            private set
            {
                if (this.____isConnect != value) ConnectStateChange?.Invoke(this, value);
                this.____isConnect = value;
            }
        }

        public async void Connect(System.Threading.CancellationToken? token = null)
        {
            var _token = token ?? GetDefaultCancellationToken();
            if (ReceivedMessageFunc == null) throw (new ArgumentNullException(nameof(ReceivedMessageFunc)));

            Log(LogType.Info, "开始连接服务器");

            if (!this._isConnect)
                clientWebSocket = new System.Net.WebSockets.ClientWebSocket();
            else
                throw new Exception("已经建立链接，请断开链接后再试");
            await clientWebSocket.ConnectAsync(new Uri("ws://message.1688.com/websocket"), _token);
            var rr = System.Threading.Tasks.Task.Run(async () =>
            {
                while (_isConnect)
                {
                    try
                    {
                        var buffer = new ArraySegment<byte>(new byte[4096]);
                        var result = await clientWebSocket.ReceiveAsync(buffer, _token);
                        if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
                        {
                            if (result.EndOfMessage)
                            {
                                var json = System.Text.Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                                var receivedMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceivedMessage>(json, serializerSettings);
                                Log(LogType.Info, "接收到消息，类型:" + receivedMessage.Type);
                                if (receivedMessage.Type == WebSocketMessageType.CONNECT_ACK)
                                {
                                    IsConnect = true;
                                    sendHeartbeat(_token);
                                }
                                else if (receivedMessage.Type == WebSocketMessageType.SERVER_PUSH)
                                {
                                    var dd = System.Threading.Tasks.Task.Run(() =>
                                    {
                                        try
                                        {
                                            var b = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceivedMessageDataBaseType>(receivedMessage.Content, serializerSettings);
                                            var ss = GetReceivedMessageData(b.Type, receivedMessage.Content);
                                            var r = ReceivedMessageFunc?.Invoke(receivedMessage.MsgSource, ss);
                                            if (r == true)
                                                sendConfirm(receivedMessage, _token);
                                        }
                                        catch (Exception ex)
                                        {
                                            Log(LogType.Error, "处理消息异常", ex);
                                        }
                                    });
                                }
                                else if (receivedMessage.Type == WebSocketMessageType.SYSTEM)
                                {
                                    SystemInfoFunc?.Invoke(receivedMessage.MsgSource, receivedMessage.Content);
                                }
                                else
                                {

                                    Log(LogType.Error, "接收到其他消息，--------------------------------------------------类型:" + receivedMessage.Type);
                                    //throw new Exception("异常");
                                }
                            }
                            else
                            {
                                throw new Exception("异常");
                            }
                        }
                        else if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Binary)
                        {
                            throw new Exception("异常");
                        }
                        else if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                        {
                            throw new Exception("异常");
                        }
                        else
                        {
                            throw new Exception("异常");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(LogType.Error, "接收数据异常", ex);
                        //throw new Exception("异常", ex);
                    }
                }
            });

            sendConnect(_token);
            //sendHeartbeat();
        }
        public async void StopConnect(System.Threading.CancellationToken? token = null)
        {
            var _token = token ?? GetDefaultCancellationToken();

            Log(LogType.Info, "开始断开连接服务器");

            if (this._isConnect)
                await clientWebSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "", _token);
        }
        public void StartReconnect(System.Threading.CancellationToken? token = null)
        {
            var _token = token ?? GetDefaultCancellationToken();

            Log(LogType.Info, "重新链接");

            //链接已经断开，重新链接
            if (!this._isConnect)
                Connect(_token);
            else if (!this.IsConnect)
                sendConnect(_token);
            else
            {
                //已经链接，不需要重新链接
            }
        }
        private System.Threading.CancellationToken GetDefaultCancellationToken()
        {
            //return System.Threading.CancellationToken.None;
            return new System.Threading.CancellationToken();
        }
        //
        bool _isConnect
        {
            get
            {
                var r = clientWebSocket != null && clientWebSocket.State == System.Net.WebSockets.WebSocketState.Open;
                if (!r) IsConnect = false;
                return r;
            }
        }
        private object lockSend = new object();
        private void send(object d, System.Threading.CancellationToken token)
        {
            try
            {
                if (this._isConnect)
                {
                    var vvv = Newtonsoft.Json.JsonConvert.SerializeObject(d, serializerSettings);
                    Log(LogType.Info, "发送消息，数据:" + vvv);
                    lock (lockSend)
                    {
                        clientWebSocket.SendAsync(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(vvv)), System.Net.WebSockets.WebSocketMessageType.Text, true, token).Wait();
                    }
                }
                else
                {
                    Log(LogType.Error, "发送消息，发现链接关闭");
                }
            }
            catch (Exception ex)
            {
                Log(LogType.Error, "发送消息时产生异常，" + ex.Message);

                //throw;
            }
        }
        private System.Threading.Timer timerSendHeartbeat;
        /// <summary>
        /// 发时心跳
        /// </summary>
        private void sendHeartbeat(System.Threading.CancellationToken token)
        {
            if (timerSendHeartbeat == null)
            {
                timerSendHeartbeat = new System.Threading.Timer(f =>
                {
                    if (!this._isConnect)
                        timerSendHeartbeat.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    else
                    {
                        Log(LogType.Info, "发送心跳请求");
                        var wsm = new WebSocketMessage
                        {
                            AppKey = _appKey,
                            Type = WebSocketMessageType.HEARTBEAT,
                            PubTime = AlibabaSDK.Utility.DateTimeHelper.GetCurrentTimestamp(),
                        };
                        wsm.Sign = sign(wsm, _clientSecret);
                        send(wsm, token);
                    }
                }, null,
                System.Threading.Timeout.Infinite,
                System.Threading.Timeout.Infinite
                );
            }

            var FETCH_PERIOD = 20;
            timerSendHeartbeat.Change(FETCH_PERIOD * 1000L, FETCH_PERIOD * 1000L);
        }
        /// <summary>
        /// 发送链接请求
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="secret"></param>
        private void sendConnect(System.Threading.CancellationToken token)
        {
            Log(LogType.Info, "发送链接请求");
            var wsm = new WebSocketMessage
            {
                AppKey = _appKey,
                Type = WebSocketMessageType.CONNECT,
                PubTime = AlibabaSDK.Utility.DateTimeHelper.GetCurrentTimestamp(),
            };
            wsm.Sign = sign(wsm, _clientSecret);
            send(wsm, token);
        }
        /// <summary>
        /// 发送消息确认
        /// </summary>
        /// <param name="message"></param>
        private void sendConfirm(ReceivedMessage message, System.Threading.CancellationToken token)
        {
            if (this._isConnect)
            {
                Log(LogType.Info, "发送接收确认信息" + message.Id);
                var wsm = new WebSocketMessage
                {
                    AppKey = _appKey,
                    Id = System.Guid.NewGuid().ToString("N"),
                    Type = WebSocketMessageType.CONFIRM,
                    PubTime = AlibabaSDK.Utility.DateTimeHelper.GetCurrentTimestamp(),
                    RelatedId = Convert.ToInt64(message.Id),
                    CostInIsv = message.CostInIsv,
                    MsgSource = message.MsgSource,
                    RelatedMsgTime = message.PubTime
                };
                //wsm.sign = sign(wsm, _clientSecret);
                send(wsm, token);
            }
        }
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private string sign(WebSocketMessage tm, string secret)
        {
            var query = new System.Text.StringBuilder(secret);
            query.Append(tm.AppKey);
            //query.Append(tm.content);
            query.Append(tm.PubTime);
            byte[] bytes = encryptMD5(query.ToString());

            return AlibabaSDK.Utility.SignatureHelper.ToHex(bytes);
        }

        private byte[] encryptMD5(string dd)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var r = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dd));
            return r;
        }
        public static ReceivedMessageDataBase GetReceivedMessageData(string type, string content)
        {
            if (Enum.TryParse<TypeDescription>(type, out var typeDescription))
                return GetReceivedMessageData(typeDescription, content);
            else
                return null;
        }
        private static System.Collections.Generic.Dictionary<TypeDescription, string> _getTypeClass;
        public static ReceivedMessageDataBase GetReceivedMessageData(TypeDescription type, string content)
        {
            //if (_getTypeClass == null)
            //{
            //    var s = new TypeDescriptionJson().JsonClass;
            //    _getTypeClass = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<TypeDescription, string>>(s);
            //}
            //var typeKey = _getTypeClass[type];
            var typeKey = type.ToString();

            var dddtype = typeof(ReceivedMessageData<>);
            var mtype = dddtype.Assembly.GetType("AlibabaSDK.WebSocketModels." + typeKey, false) ?? typeof(object);
            var ssss = dddtype.MakeGenericType(mtype);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(content, ssss) as ReceivedMessageDataBase;
        }
    }
    class WebSocketMessageBase
    {/// <summary>
     /// 系统内部字段
     /// </summary>
        [JsonProperty("relatedId")]
        public long? RelatedId { get; set; }
        /// <summary>
        /// 应用 AppKey
        /// </summary>
        [JsonProperty("appKey")]
        public string AppKey { get; set; }
        /// <summary>
        /// isv端消息处理耗时
        /// </summary>
        [JsonProperty("costInIsv")]
        public int? CostInIsv { get; set; }
        /// <summary>
        /// 消息id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// * 数据来源。包括
        /// * MOCK: 测试数据。使用消息测试工具产生。
        /// * REAL: 真实数据。此字段为空时默认为真实数据。
        /// </summary>
        [JsonProperty("msgSource")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MsgSource MsgSource { get; set; }
        /// <summary>
        /// 消息推送时间
        /// </summary>
        [JsonProperty("pubTime")]
        public long? PubTime { get; set; }

        /// <summary>
        /// webocket消息类型
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public WebSocketMessageType Type { get; set; }
    }
    class WebSocketMessage : WebSocketMessageBase
    {
        [JsonProperty("relatedMsgTime")]
        public long? RelatedMsgTime { get; set; }
        ///**
        // * 签名
        // */
        [JsonProperty("sign")]
        public String Sign { get; set; }
        ///**
        // * 应用 AppSecret
        // */
        //String secret;
        ///**
        // * 消息内容，json串格式
        // */
        //private String content;
    }
    class ReceivedMessage : WebSocketMessageBase
    {
        /// <summary>
        /// 消息内容，json串格式
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    enum WebSocketMessageType
    {
        /// <summary>
        /// 客户端发起建连请求 	
        /// </summary>
        CONNECT,
        /// <summary>
        /// 服务端建连成功返回	
        /// </summary>
        CONNECT_ACK,
        /// <summary>
        /// 客户端心跳	
        /// </summary>
        HEARTBEAT,
        /// <summary>
        /// 客户端消息消费成功确认	
        /// </summary>
        CONFIRM,
        /// <summary>
        /// 服务端消息推送	
        /// </summary>
        SERVER_PUSH,
        CLIENT_PUSH,
        /// <summary>
        /// 链接关闭	
        /// </summary>
        CLOSE,
        /// <summary>
        /// 服务端异常
        /// </summary>
        SYSTEM
    }
    public enum MsgSource
    {
        /// <summary>
        /// 真实数据。此字段为空时默认为真实数据。
        /// </summary>
        REAL,
        /// <summary>
        /// 测试数据。使用消息测试工具产生。
        /// </summary>
        MOCK

    }
    public enum LogType
    {
        Info,
        Error
    }

    namespace WebSocketModels
    {
        public class ReceivedMessageDataBaseType
        {
            /// <summary>
            /// 消息类型，每个业务消息都唯一对应一个类型，参考业务消息的类型定义 
            /// </summary>
            [JsonProperty("type")]
            [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
            public TypeDescription Type { get; set; }
        }
        public abstract class ReceivedMessageDataBase : ReceivedMessageDataBaseType
        {
            /// <summary>
            /// </summary>
            [JsonProperty("bizKey")]
            public string BizKey { get; set; }
            /// <summary>
            /// 消息推送时间
            /// </summary>
            [JsonConverter(typeof(UnixTimestampFormatConverter))]
            [JsonProperty("gmtBorn")]
            public System.DateTimeOffset GmtBorn { get; set; }
            /// <summary>
            /// 消息ID，消息唯一性标识
            /// </summary>
            [JsonProperty("msgId")]
            public long MsgId { get; set; }
            /// <summary>
            /// memberId
            /// </summary>
            [JsonProperty("userInfo")]
            public string UserInfo { get; set; }
            /// <summary>
            /// memberId
            /// </summary>
            [JsonProperty("extraInfo")]
            public System.Collections.Generic.Dictionary<string, string> ExtraInfo { get; set; }



            public string TypeDescription { get { return getTypeDescription(this.Type); } }

            private static System.Collections.Generic.Dictionary<TypeDescription, string> _getTypeDescription = null;
            private static string getTypeDescription(TypeDescription type)
            {
                if (_getTypeDescription == null)
                {
                    var s = new TypeDescriptionJson().JsonDescription;
                    _getTypeDescription = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<TypeDescription, string>>(s);
                }
                if (_getTypeDescription != null && _getTypeDescription.ContainsKey(type)) return _getTypeDescription[type];
                else return string.Empty;
            }
            public string ToJson()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }
        }
        public class ReceivedMessageData<T> : ReceivedMessageDataBase
        {
            /// <summary>
            /// 具体推送的业务消息数据，json格式，字段说明，参考各个业务消息说明
            /// </summary>
            [JsonProperty("data")]
            public T Data { get; set; }
        }

    }
}