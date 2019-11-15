# AlibabaSDKFor1688


[![Build status](https://ci.appveyor.com/api/projects/status/7e93jpto69kkgf6t?svg=true)](https://ci.appveyor.com/project/mccj/alibabasdkfor1688)
[![MyGet](https://img.shields.io/myget/mccj/vpre/AlibabaSDKFor1688.svg)](https://myget.org/feed/mccj/package/nuget/AlibabaSDKFor1688)
[![NuGet](https://img.shields.io/nuget/v/AlibabaSDKFor1688.svg)](https://www.nuget.org/packages/AlibabaSDKFor1688)
![MIT License](https://img.shields.io/badge/license-MIT-orange.svg)


����ͰͿ���ƽ̨SDK C#�汾
http://open.1688.com/


## ʹ������
```C#
//api����
    var alibabaClient = new AlibabaSDK.AlibabaStandardApiClient("appKey", "appSecret");
    //������Ȩ��Ϣ
    alibabaClient.SetAccessToken("accessToken");
    //��������鿴
    var order = alibabaClient.AlibabaTradeGetBuyerView("1688", 1111);



//��Ϣ����
    var alibabaWebSocketClient = new AlibabaWebSocketClient(ClientInfo.ClientId, ClientInfo.ClientSecret)
    {
        ReceivedMessageFunc = (msgSource, data) =>
        {//�Խ��յ�����Ϣ���д���
            var r = ��Ϣ����(data);
            return r;
        }
    };
    alibabaWebSocketClient.Connect();
```
