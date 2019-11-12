using AlibabaSDK.Utility;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AlibabaSDK
{
    public partial class AlibabaApiClientCommonEnum
    {
        public static class AlibabaOpenplatformTradeModelOrderStatus
        {
            /// <summary>
            /// 1.等待买家付款
            /// </summary>
            public static string 等待买家付款 { get; } = "waitbuyerpay";
            /// <summary>
            /// 2.等待卖家发货
            /// </summary>
            public static string 等待卖家发货 { get; } = "waitsellersend";
            /// <summary>
            /// 3.等待物流公司揽件
            /// </summary>
            public static string 等待物流公司揽件 { get; } = "waitlogisticstakein";
            /// <summary>
            /// 4.等待买家收货
            /// </summary>
            public static string 等待买家收货 { get; } = "waitbuyerreceive";
            /// <summary>
            /// 5.等待买家签收
            /// </summary>
            public static string 等待买家签收 { get; } = "waitbuyersign";
            /// <summary>
            /// 6.买家已签收
            /// </summary>
            public static string 买家已签收 { get; } = "signinsuccess";
            /// <summary>
            /// 7.已收货
            /// </summary>
            public static string 已收货 { get; } = "confirm_goods";
            /// <summary>
            /// 8.交易成功
            /// </summary>
            public static string 交易成功 { get; } = "success";
            /// <summary>
            /// 交易取消
            /// </summary>
            public static string 交易取消 { get; } = "cancel";
            /// <summary>
            /// 交易终止
            /// </summary>
            public static string 交易终止 { get; } = "terminated";
            public static string ToNotes(string status)
            {
                if (string.IsNullOrWhiteSpace(status)) return "";
                var s = "waitbuyerpay:等待买家付款;waitsellersend:等待卖家发货;waitlogisticstakein:等待物流公司揽件;waitbuyerreceive:等待买家收货;waitbuyersign:等待买家签收;signinsuccess:买家已签收;confirm_goods:已收货;success:交易成功;cancel:交易取消;terminated:交易终止;未枚举:其他状态";
                var ddd = s.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(f => f.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(f => f[0], f => f[1]);
                return ddd.ContainsKey(status) ? ddd[status] : "其他状态";
            }
        }
        public static class AlibabaLogisticsOpenPlatformLogisticsOrderStatus
        {
            /// <summary>
            /// 未受理
            /// </summary>
            public static string 未受理 { get; } = "WAITACCEPT";
            /// <summary>
            /// 已撤销
            /// </summary>
            public static string 已撤销 { get; } = "CANCEL";
            /// <summary>
            /// 已受理
            /// </summary>
            public static string 已受理 { get; } = "ACCEPT";
            /// <summary>
            /// 运输中
            /// </summary>
            public static string 运输中 { get; } = "TRANSPORT";
            /// <summary>
            /// 揽件失败
            /// </summary>
            public static string 揽件失败 { get; } = "NOGET";
            /// <summary>
            /// 已签收
            /// </summary>
            public static string 已签收 { get; } = "SIGN";
            /// <summary>
            /// 签收异常
            /// </summary>
            public static string 签收异常 { get; } = "UNSIGN";
            
            public static string ToNotes(string status)
            {
                if (string.IsNullOrWhiteSpace(status)) return "";
                var s = "WAITACCEPT:未受理;CANCEL:已撤销;ACCEPT:已受理;TRANSPORT:运输中;NOGET:揽件失败;SIGN:已签收;UNSIGN:签收异常";
                var ddd = s.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(f => f.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(f => f[0], f => f[1]);
                return ddd.ContainsKey(status) ? ddd[status] : "其他状态";
            }
        }
    }
}