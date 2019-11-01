using AlibabaSDK;
using AlibabaSDK.Utility;
using System;
using Xunit;
using Xunit.Abstractions;

namespace AlibabaSDKFor1688Test
{
    public class OAuthApiTest
    {
        protected readonly ITestOutputHelper Output;

        public OAuthApiTest(ITestOutputHelper tempOutput)
        {
            Output = tempOutput;
        }

        [Fact]
        public void TestGetAuthorizationUrl()
        {
            var authUrl = OAuthApi.GetAuthorizeUrl(ClientInfo.ClientId, state: "eastmall.vip");
            Output.WriteLine(authUrl);
        }

        [Fact]
        public void TestGetAccessToken()
        {
            var redirect_uri = @"https://auth.1688.com/auth/authCode.htm";
            var pre_auth_code = "c55426c9-1604-406f-8379-82003fb61132";

            var accessToken = OAuthApi.GetAccessToken(clientId: ClientInfo.ClientId, clientSecret: ClientInfo.ClientSecret, redirectUrl: redirect_uri, preAuthCode: pre_auth_code);
            Output.WriteLine("accessToken {0} , refreshToken {1}", accessToken.AccessToken, accessToken.RefreshToken);
        }

        [Fact]
        public void TestRefreshAccessToken()
        {
            var accessToken = OAuthApi.RefreshAccessToken(
                clientId: ClientInfo.ClientId,
                clientSecret: ClientInfo.ClientSecret,
                refreshToken: ClientInfo.RefreshToken
                );
            Output.WriteLine("accessToken {0} , refreshToken {1}", accessToken.AccessToken, accessToken.RefreshToken);
        }

        [Fact]
        public void TestRenewRefreshToken()
        {
            var token = OAuthApi.RenewRefreshToken(clientId: ClientInfo.ClientId, clientSecret: ClientInfo.ClientSecret, refreshToken: ClientInfo.RefreshToken, accessToken: ClientInfo.AccessToken);
            Output.WriteLine("accessToken {0} , refreshToken {1}, time {2}", token.AccessToken, token.RefreshToken, token.RefreshTokenTimeout);
        }

        [Fact]
        public void TestFormatDateForOcean()
        {
            var date = DateTime.Now;
            Output.WriteLine(DateTimeHelper.FormatForOcean(date));
        }
        [Fact]
        public void TestGetCurrentTimestamp()
        {
            //Output.WriteLine(DateTimeHelper.GetCurrentTimestamp().ToString());
            Output.WriteLine(DateTimeHelper.GetCurrentTimestamp().ToString());
        }
        //[Fact]
        //public void TestFormatDateForOceanString()
        //{
        //    var date = Com.Alibaba.Helpers.StringValidate.StringValidateForDatetimeOcean("20180518144843", true);
        //    Console.WriteLine(date);
        //}

        //[Fact]
        //public void TestEnum()
        //{
        //    var a = Enum.GetNames(typeof(Com.Alibaba.Trade.BizType));
        //    foreach (var b in a)
        //    {
        //        Console.WriteLine($" {b}=> {b}");
        //    }
        //}

        //[Fact]
        //public void TestGetOrderListOnSellerView()
        //{
        //    try
        //    {
        //        var requestPolicy = new Com.Alibaba.Entities.Request.RequestPolicy
        //        {

        //            ClientId = "*****",//
        //            ClientSecret = "*****",// 
        //            AccessToken = "c4cdb98e-8621-4fa5-80a4-2dbc7bd7c0e6",
        //            ApiRoot = "openapi",
        //            ApiVersion = "1",
        //            ApiNamespace = "com.alibaba.trade",
        //            ApiName = "alibaba.trade.getSellerOrderList",
        //            Protocol = Com.Alibaba.Protocol.param2,
        //            SessionType = SessionType.Prod,
        //            RequestSchema = UriSchema.https
        //        };

        //        Com.Alibaba.Trade.Containers.AccessTokenContainer.Register(requestPolicy.ClientId, requestPolicy.ClientSecret, requestPolicy.AccessToken, 36000, 36000, "41227eb4-520e-48e0-a432-7d17c870bf6c", "http://localhost:12305");

        //        var reqModel = new Com.Alibaba.Trade.Entities.Request.ReqModelOrderListForSeller();

        //        var result = Com.Alibaba.Trade.TradeAPIs.OrderApi.OrderListOnSellerViewRetrieveRowString(requestPolicy, reqModel);

        //        //var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);

        //        //dynamic o = result.ReturnObject;

        //        //var s = new Dictionary<string, object>(o._dictionary);

        //        foreach (var i in result)
        //        {
        //            Console.WriteLine($"  {i.Key}  =>  {i.Value}");
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.ToString());
        //    }

        //}

        //[Fact]
        //public void TestGetOrderOnSellerView()
        //{
        //    try
        //    {
        //        var requestPolicy = new Com.Alibaba.Entities.Request.RequestPolicy
        //        {

        //            ClientId = "*****",//
        //            ClientSecret = "*****",// 
        //            AccessToken = "c4cdb98e-8621-4fa5-80a4-2dbc7bd7c0e6",
        //            ApiRoot = "openapi",
        //            ApiVersion = "1",
        //            ApiNamespace = "com.alibaba.trade",
        //            ApiName = "alibaba.trade.get.sellerView",
        //            Protocol = Com.Alibaba.Protocol.param2,
        //            SessionType = SessionType.Prod,
        //            RequestSchema = UriSchema.https
        //        };

        //        Com.Alibaba.Trade.Containers.AccessTokenContainer.Register(requestPolicy.ClientId, requestPolicy.ClientSecret, requestPolicy.AccessToken, 36000, 36000, "41227eb4-520e-48e0-a432-7d17c870bf6c", "http://localhost:12305");

        //        var reqModel = new Com.Alibaba.Trade.Entities.Request.ReqModelOrderDetailForSeller();

        //        reqModel.orderId = 159310337253103128L;

        //        var result = Com.Alibaba.Trade.TradeAPIs.OrderApi.OrderDetailOnSellerViewRetrieveRowString(requestPolicy, reqModel);

        //        foreach (var i in result)
        //        {
        //            Console.WriteLine($"  {i.Key}  =>  {i.Value}");

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.ToString());
        //    }
        //}

        //[Fact]
        //public void TestSer()
        //{
        //    try
        //    {
        //        JavaScriptSerializer js = new JavaScriptSerializer();
        //        var returnText = "{    \"result\": [        {    \" guaranteesTerms\":null,        \"nativeLogistics\": {                \"area\": \"义乌市\",                \"areaCode\": \"330782\",                \"city\": \"金华市\",                \"province\": \"浙江省\",                \"townCode\": \"330782004\",                \"town\": \"北苑街道\"            },            \"productItems\": [                {                    \"cargoNumber\": \"4571300432243\",                    \"itemAmount\": 19.8,                    \"name\": \"马桶刷 厕所清洁刷 强力去污  228-H-3222 H-3223 H-3224\",                    \"price\": 19.8,                    \"productID\": 569530644133,                    \"productImgUrl\": [                        \"http://cbu01.alicdn.com/img/order/trading/821/301/452733013951/8864457926_2092783878.80x80.jpg\",                        \"http://cbu01.alicdn.com/img/order/trading/821/301/452733013951/8864457926_2092783878.jpg\"                    ],                    \"productSnapshotUrl\": \"https://trade.1688.com/order/offer_snapshot.htm?order_entry_id=159310337254103128\",                    \"quantity\": 1,                    \"refund\": 0,                    \"skuID\": 3660214956846,                    \"status\": \"waitsellersend\",                    \"subItemID\": 159310337254103128,                    \"type\": \"common\",                    \"unit\": \"个\",                    \"guaranteesTerms\": [],                    \"skuInfos\": [                        {                            \"name\": \"颜色\",                            \"value\": \"黄色\"                        }                    ],                    \"entryDiscount\": 0,                    \"specId\": \"36c58a69219820709b9475e70bd789d4\",                    \"quantityFactor\": 1,                    \"statusStr\": \"等待卖家发货\",                    \"logisticsStatus\": 1,                    \"gmtCreate\": \"20180514180644000+0800\",                    \"gmtModified\": \"20180514180655000+0800\",                    \"subItemIDString\": \"159310337254103128\"                },                {                    \"cargoNumber\": \"4571300432229\",                    \"itemAmount\": 19.8,                    \"name\": \"马桶刷 厕所清洁刷 强力去污  228-H-3222 H-3223 H-3224\",                    \"price\": 19.8,                    \"productID\": 569530644133,                    \"productImgUrl\": [                        \"http://cbu01.alicdn.com/img/order/trading/821/301/552733013951/8884303861_2092783878.80x80.jpg\",                        \"http://cbu01.alicdn.com/img/order/trading/821/301/552733013951/8884303861_2092783878.jpg\"                    ],                    \"productSnapshotUrl\": \"https://trade.1688.com/order/offer_snapshot.htm?order_entry_id=159310337255103128\",                    \"quantity\": 1,                    \"refund\": 0,                    \"skuID\": 3660214956845,                    \"status\": \"waitsellersend\",                    \"subItemID\": 159310337255103128,                    \"type\": \"common\",                    \"unit\": \"个\",                    \"guaranteesTerms\": [],                    \"skuInfos\": [                        {                            \"name\": \"颜色\",                            \"value\": \"白色\"                        }                    ],                    \"entryDiscount\": 0,                    \"specId\": \"a63c985e3358d02b842322fc287be521\",                    \"quantityFactor\": 1,                    \"statusStr\": \"等待卖家发货\",                    \"logisticsStatus\": 1,                    \"gmtCreate\": \"20180514180644000+0800\",                    \"gmtModified\": \"20180514180655000+0800\",                    \"subItemIDString\": \"159310337255103128\"                },                {                    \"itemAmount\": 90,                    \"name\": \"长柄半球L型马桶刷\",                    \"price\": 9,                    \"productID\": 569659293154,                    \"productImgUrl\": [                        \"http://cbu01.alicdn.com/img/order/trading/821/301/652733013951/8900939241_2092783878.310x310.80x80.jpg\",                        \"http://cbu01.alicdn.com/img/order/trading/821/301/652733013951/8900939241_2092783878.310x310.jpg\"                    ],                    \"productSnapshotUrl\": \"https://trade.1688.com/order/offer_snapshot.htm?order_entry_id=159310337256103128\",                    \"quantity\": 10,                    \"refund\": 0,                    \"status\": \"waitsellersend\",                    \"subItemID\": 159310337256103128,                    \"type\": \"common\",                    \"unit\": \"个\",                    \"guaranteesTerms\": [],                    \"productCargoNumber\": \"291-TL125\",                    \"entryDiscount\": 0,                    \"quantityFactor\": 1,                    \"statusStr\": \"等待卖家发货\",                    \"logisticsStatus\": 1,                    \"gmtCreate\": \"20180514180644000+0800\",                    \"gmtModified\": \"20180514180655000+0800\",                    \"subItemIDString\": \"159310337256103128\"                }            ],            \"tradeTerms\": [                {                    \"payStatus\": \"2\",                    \"payTime\": \"20180514180654000+0800\",                    \"payWay\": \"1\",                    \"phasAmount\": 129.6,                    \"phase\": 2272756232103128,                    \"cardPay\": true,                    \"expressPay\": true                }            ],            \"orderRateInfo\": {                \"buyerRateStatus\": 5,                \"sellerRateStatus\": 5            },            \"baseInfo\": {                \"businessType\": \"cn\",                \"buyerID\": \"b2b-251028315119f\",                \"createTime\": \"20180514180644000+0800\",                \"id\": 159310337253103128,                \"modifyTime\": \"20180514180655000+0800\",                \"payTime\": \"20180514180654000+0800\",                \"refund\": 0,                \"sellerID\": \"b2b-39414139407d68f\",                \"shippingFee\": 0,                \"status\": \"waitsellersend\",                \"totalAmount\": 129.6,                \"discount\": 0,                \"buyerContact\": {                    \"phone\": \"\",                    \"imInPlatform\": \"zamen2005\",                    \"name\": \"张华辉\"                },                \"sellerContact\": {                    \"phone\": \"86-579-85698050\",                    \"imInPlatform\": \"yunliannet\",                    \"name\": \"余先球\",                    \"mobile\": \"18057940892\",                    \"companyName\": \"义乌市云联网络科技有限公司\"                },                \"tradeType\": \"50060\",                \"refundPayment\": 0,                \"idOfStr\": \"159310337253103128\",                \"alipayTradeId\": \"2018051421001008320552427523\",                \"receiverInfo\": {                    \"toFullName\": \"哲明\",                    \"toDivisionCode\": \"330782\",                    \"toPost\": \"\",                    \"toTownCode\": \"330782004\",                    \"toArea\": \"浙江省 金华市 义乌市 北苑街道\"                },                \"buyerLoginId\": \"zamen2005\",                \"sellerLoginId\": \"yunliannet\",                \"buyerUserId\": 25102831,                \"sellerUserId\": 3941413940,                \"buyerAlipayId\": \"2088002190839321\",                \"sellerAlipayId\": \"2088002130799577\",                \"sumProductPayment\": 129.6,                \"stepPayAll\": false,                \"overSeaOrder\": false,                \"sellerOrder\": false,                \"flowTemplateCode\": \"assureTradeAlipay\"            }        }    ],    \"totalRecord\": 1}";
        //        Com.Alibaba.Trade.Entities.OrderListForSellerContainer result = js.Deserialize<Com.Alibaba.Trade.Entities.OrderListForSellerContainer>(returnText);

        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex);

        //    }
        //}


    }
}
