# AlibabaSDKFor1688


[![Build status](https://ci.appveyor.com/api/projects/status/7e93jpto69kkgf6t?svg=true)](https://ci.appveyor.com/project/mccj/alibabasdkfor1688)
[![MyGet](https://img.shields.io/myget/mccj/vpre/AlibabaSDKFor1688.svg)](https://myget.org/feed/mccj/package/nuget/AlibabaSDKFor1688)
[![NuGet](https://img.shields.io/nuget/v/AlibabaSDKFor1688.svg)](https://www.nuget.org/packages/AlibabaSDKFor1688)
![MIT License](https://img.shields.io/badge/license-MIT-orange.svg)


阿里巴巴开放平台SDK C#版本
http://open.1688.com/


## 使用例子
```C#
//api调用
    var alibabaClient = new AlibabaSDK.AlibabaStandardApiClient("appKey", "appSecret");
    //设置授权信息
    alibabaClient.SetAccessToken("accessToken");
    //订单详情查看
    var order = alibabaClient.AlibabaTradeGetBuyerView("1688", 1111);



//消息接收
    var alibabaWebSocketClient = new AlibabaWebSocketClient(ClientInfo.ClientId, ClientInfo.ClientSecret)
    {
        ReceivedMessageFunc = (msgSource, data) =>
        {//对接收到的消息进行处理
            var r = 消息处理(data);
            return r;
        }
    };
    alibabaWebSocketClient.Connect();
```
