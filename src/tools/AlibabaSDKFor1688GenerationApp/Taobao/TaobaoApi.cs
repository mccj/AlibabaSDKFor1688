using ConsoleApp2.Taobao.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ConsoleApp2.Taobao
{

    public class TaobaoApi
    {
        private System.Net.Http.HttpClient _httpClient;
        public TaobaoApi()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Cookie", "_tb_token_=e734013eb3593");
        }
        //public async Task<BaseResponse<System.DateTime>> GetSystemTimestamp()
        //{
        //    var response = await _httpClient.GetAsync("https://gw.open.1688.com/console/service/getSystemTimestamp.json");
        //    var result = await response.Content.ReadAsAsync<BaseResponse<System.DateTime>>();
        //    return result;
        //}
        public async Task<CatelogTree[]> GetCatelogConfig()
        {
            var response = await _httpClient.GetAsync("https://open.taobao.com/handler/document/getApiCatelogConfig.json?_tb_token_=e734013eb3593");
            var json = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<ApiCatelogConfigData>>(json);
            var r = result?.Data?.TreeCategories?.SelectMany(f => f.CatelogTrees).ToArray();
            return r;
        }  
        public async Task<DocumentData> GetDocument(int docId,int docType)
        {
            var response = await _httpClient.GetAsync($"https://open.taobao.com/handler/document/getDocument.json?isEn=false&docId={docId}&docType={docType}&_tb_token_=e734013eb3593");
            var json = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<DocumentData>>(json);
            //var r= result?.Data?.TreeCategories?.SelectMany(f=>f.CatelogTrees).ToArray();
            return result?.Data;
        }
        //public async Task<BaseResponse<ArgumentData>> GetApiArguments(string @namespace, string apiname, int version)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("https://gw.open.1688.com/console/service/getApiArguments.json", new { @namespace, apiname, version });
        //    var result = await response.Content.ReadAsAsync<BaseResponse<ArgumentData>>();
        //    return result;
        //}
        //public async Task<Base2Response<CategoryResult[]>> GetAllApiCategory()
        //{
        //    var response = await _httpClient.GetAsync("https://open.1688.com/api/data/getAllApiCategory.json");
        //    var result = await response.Content.ReadAsAsync<Base2Response<CategoryResult[]>>();
        //    return result;
        //}
        //public async Task<Base2Response<ListByCategoryResult[]>> GetAopApiListByCategory(string aopApiCategory)
        //{
        //    var response = await _httpClient.GetAsync("https://open.1688.com/api/data/getAopApiListByCategory.json?aopApiCategory=" + aopApiCategory);
        //    var result = await response.Content.ReadAsAsync<Base2Response<ListByCategoryResult[]>>();
        //    return result;
        //}
        //public async Task<Base2Response<DetailResult>> GetApiDetail(string @namespace, string apiname, int version)
        //{
        //    var response = await _httpClient.GetAsync($"https://open.1688.com/api/data/getApiDetail.json?_input_charset=UTF-8&namespace={@namespace}&name={apiname}&version={version}");
        //    var result = await response.Content.ReadAsAsync<Base2Response<DetailResult>>();
        //    return result;
        //}
        //public async Task<Base2Response<ModelInfoResult[]>> GetModelInfo(string @namespace, string apiname, int version, string typeName)
        //{
        //    var response = await _httpClient.GetAsync($"https://open.1688.com/api/data/getModelInfo.json?_input_charset=UTF-8&type=2&namespace={@namespace}&apiname={apiname}&version={version}&typeName={typeName}");
        //    //var result1 = await response.Content.ReadAsStringAsync();
        //    var result = await response.Content.ReadAsAsync<Base2Response<ModelInfoResult[]>>();
        //    return result;
        //}



        //public async Task<Base2Response<ddd.TopicGroupsResult[]>> GetTopicGroups()
        //{
        //    var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getTopicGroups.json?_input_charset=UTF-8");
        //    var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicGroupsResult[]>>();
        //    return result;
        //}
        //public async Task<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>> GetTopicsByGroupAndOwner(string topicGroup)
        //{
        //    var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getTopicsByGroupAndOwner.json?_input_charset=UTF-8&topicGroup=" + topicGroup);
        //    var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>>();
        //    return result;
        //}
        //public async Task<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>> GetAllTopics()
        //{
        //    var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getAllTopics.json?_input_charset=UTF-8");
        //    var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>>();
        //    return result;
        //}
        //public async Task<Base2Response<ddd.TopicResult>> GetTopic(string topicId)
        //{
        //    var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getTopic.json?_input_charset=UTF-8&topicId=" + topicId);
        //    var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicResult>>();
        //    return result;
        //}


        //public async Task<Datum[]> GetSolutionApiAndMessageListDetail()
        //{
        //    var response = await _httpClient.GetAsync($"https://open.1688.com/solution/data/getSolutionDetail.jsonp?solutionKey=1513248184893&callback=jsonp");
        //    var result1 = await response.Content.ReadAsStringAsync();
        //    var ss = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(result1.Remove(0, 5).Trim('(', ')'));
        //    var ddjson = ss.SelectToken("$.result.solutionDescList[?(@subCategory=='apiAndMessageList')].content").ToObject<string>();
        //    var ss1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(ddjson);
        //    var ss2 = ss1.SelectTokens("$.apis[*].modules[*]");
        //    var result = ss2.Select(f => new Datum
        //    {
        //        //description = f.Value<string>("description"),
        //        //fullName = f.Value<string>("fullName"),
        //        Name = f.Value<string>("name"),
        //        Namespace = f.Value<string>("namespace"),
        //        Version = f.Value<int>("version")
        //    }).ToArray();
        //    return result;
        //}
    }

    namespace Models
    {
        public class Metadata
        {
        }

        public class CatelogList
        {

            [JsonProperty("docType")]
            public int DocType { get; set; }

            [JsonProperty("docUrl")]
            public string DocUrl { get; set; }

            [JsonProperty("docId")]
            public int DocId { get; set; }

            [JsonProperty("tips")]
            public string Tips { get; set; }

            [JsonProperty("selected")]
            public bool Selected { get; set; }

            [JsonProperty("subName")]
            public string SubName { get; set; }

            [JsonProperty("children")]
            public object[] Children { get; set; }

            [JsonProperty("pid")]
            public int Pid { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }
        }

        public class CatelogTree
        {

            [JsonProperty("catelogList")]
            public CatelogList[] CatelogList { get; set; }

            [JsonProperty("treeName")]
            public string TreeName { get; set; }

            [JsonProperty("selected")]
            public bool Selected { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }
        }

        public class TreeCategory
        {

            [JsonProperty("displayOrder")]
            public int DisplayOrder { get; set; }

            [JsonProperty("docUrl")]
            public string DocUrl { get; set; }

            [JsonProperty("catelogTrees")]
            public CatelogTree[] CatelogTrees { get; set; }

            [JsonProperty("selected")]
            public bool Selected { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }
        }

        public class ApiCatelogConfigData
        {

            [JsonProperty("docIndex")]
            public string DocIndex { get; set; }

            [JsonProperty("treeCategories")]
            public TreeCategory[] TreeCategories { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }
        }




        /// ////////////////////////////////////////////////////////////////
        public class ErrorCodeEx
        {

            [JsonProperty("solution")]
            public string Solution { get; set; }

            [JsonProperty("errorMsg")]
            public string ErrorMsg { get; set; }

            [JsonProperty("errorCode")]
            public string ErrorCode { get; set; }
        }

        public class EnvConfig
        {

            [JsonProperty("httpUrl")]
            public string HttpUrl { get; set; }

            [JsonProperty("httpsUrl")]
            public string HttpsUrl { get; set; }

            [JsonProperty("env")]
            public string Env { get; set; }
        }

        public class ResponseParam
        {

            [JsonProperty("subParams")]
            public ResponseParam[] SubParams { get; set; }

            [JsonProperty("paramOrder")]
            public object ParamOrder { get; set; }

            [JsonProperty("demoValue")]
            public string DemoValue { get; set; }

            [JsonProperty("maxListSize")]
            public object MaxListSize { get; set; }

            [JsonProperty("fileExt")]
            public string FileExt { get; set; }

            [JsonProperty("needShortAuthority")]
            public object NeedShortAuthority { get; set; }

            [JsonProperty("writeLevel")]
            public object WriteLevel { get; set; }

            [JsonProperty("fieldsReadLevel")]
            public string FieldsReadLevel { get; set; }

            [JsonProperty("required")]
            public bool Required { get; set; }

            [JsonProperty("maxLength")]
            public object MaxLength { get; set; }

            [JsonProperty("parentId")]
            public string ParentId { get; set; }

            [JsonProperty("maxValue")]
            public object MaxValue { get; set; }

            [JsonProperty("minValue")]
            public object MinValue { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("defaultValue")]
            public string DefaultValue { get; set; }
        }

        public class SdkDemo
        {

            [JsonProperty("descprition")]
            public string Descprition { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class ApplyScope
        {

            [JsonProperty("descprition")]
            public string Descprition { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Label
        {

            [JsonProperty("clickUrl")]
            public string ClickUrl { get; set; }

            [JsonProperty("tips")]
            public string Tips { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }
        }

        public class Tool
        {

            [JsonProperty("descprition")]
            public string Descprition { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
        public class DocumentData
        {

            [JsonProperty("errorCodes")]
            public ErrorCodeEx[] ErrorCodes { get; set; }

            [JsonProperty("envConfigs")]
            public EnvConfig[] EnvConfigs { get; set; }

            [JsonProperty("publicParams")]
            public ResponseParam[] PublicParams { get; set; }

            [JsonProperty("publicResponseParams")]
            public ResponseParam[] PublicResponseParams { get; set; }

            [JsonProperty("responseParams")]
            public ResponseParam[] ResponseParams { get; set; }

            [JsonProperty("sdkDemos")]
            public SdkDemo[] SdkDemos { get; set; }

            [JsonProperty("applyScopes")]
            public ApplyScope[] ApplyScopes { get; set; }

            [JsonProperty("faqs")]
            public object[] Faqs { get; set; }

            [JsonProperty("apiErrDemoXml")]
            public string ApiErrDemoXml { get; set; }

            [JsonProperty("apiErrDemoJson")]
            public string ApiErrDemoJson { get; set; }

            [JsonProperty("msgDemo")]
            public string MsgDemo { get; set; }

            [JsonProperty("labels")]
            public Label[] Labels { get; set; }

            [JsonProperty("rspSampleSimplifyJson")]
            public string RspSampleSimplifyJson { get; set; }

            [JsonProperty("apiChineseName")]
            public string ApiChineseName { get; set; }

            [JsonProperty("rspSampleXml")]
            public string RspSampleXml { get; set; }

            [JsonProperty("rspSampleJson")]
            public string RspSampleJson { get; set; }

            [JsonProperty("tools")]
            public Tool[] Tools { get; set; }

            [JsonProperty("requestParams")]
            public ResponseParam[] RequestParams { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }


        public class BaseResponse<T>
        {

            [JsonProperty("errorLevel")]
            public string ErrorLevel { get; set; }

            [JsonProperty("httpStatus")]
            public string HttpStatus { get; set; }

            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }

            [JsonProperty("errMsg")]
            public string ErrMsg { get; set; }

            [JsonProperty("msg")]
            public string Msg { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("data")]
            public T Data { get; set; }
        }



        //    public class Datum
        //    {

        //        [JsonProperty("namespace")]
        //        public string Namespace { get; set; }

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("version")]
        //        public int Version { get; set; }
        //    }
        //    public class Field
        //    {

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("type")]
        //        public string Type { get; set; }

        //        [JsonProperty("required")]
        //        public bool Required { get; set; }

        //        [JsonProperty("description")]
        //        public string Description { get; set; }

        //        [JsonProperty("sampleValue")]
        //        public string SampleValue { get; set; }
        //    }
        //    public class Domain
        //    {

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("fields")]
        //        public Field[] Fields { get; set; }
        //    }
        //    public class Param
        //    {

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("type")]
        //        public string Type { get; set; }

        //        [JsonProperty("required")]
        //        public bool Required { get; set; }

        //        [JsonProperty("description")]
        //        public string Description { get; set; }

        //        [JsonProperty("defaultValue")]
        //        public string DefaultValue { get; set; }

        //        [JsonProperty("sampleValue")]
        //        public string SampleValue { get; set; }
        //    }
        //    public class ArgumentData
        //    {

        //        [JsonProperty("domains")]
        //        public System.Collections.Generic.Dictionary<string, Domain> Domains { get; set; }

        //        [JsonProperty("params")]
        //        public Param[] Params { get; set; }
        //    }

        //    public class CategoryResult
        //    {

        //        [JsonProperty("aopCategoryCode")]
        //        public string AopCategoryCode { get; set; }

        //        [JsonProperty("aopCategoryName")]
        //        public string AopCategoryName { get; set; }
        //    }


        //    public class ListByCategoryResult
        //    {

        //        [JsonProperty("oceanApiId")]
        //        public string OceanApiId { get; set; }

        //        [JsonProperty("namespace")]
        //        public string Namespace { get; set; }

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("version")]
        //        public int Version { get; set; }

        //        [JsonProperty("description")]
        //        public string Description { get; set; }

        //        [JsonProperty("gmtModified")]
        //        public object GmtModified { get; set; }

        //        [JsonProperty("displayName")]
        //        public string DisplayName { get; set; }

        //        [JsonProperty("aopCategoryCode")]
        //        public string AopCategoryCode { get; set; }
        //    }


        //    public class ApiErrorCodeVOList
        //    {

        //        [JsonProperty("code")]
        //        public string Code { get; set; }

        //        [JsonProperty("desc")]
        //        public string Desc { get; set; }

        //        [JsonProperty("howToFix")]
        //        public string HowToFix { get; set; }
        //    }

        //    public class ApiDocSampleVOList
        //    {

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("sample")]
        //        public string Sample { get; set; }

        //        [JsonProperty("type")]
        //        public object Type { get; set; }
        //    }
        //    public class DetailResult
        //    {

        //        [JsonProperty("namespace")]
        //        public string Namespace { get; set; }

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("version")]
        //        public int Version { get; set; }

        //        [JsonProperty("oceanApiId")]
        //        public string OceanApiId { get; set; }

        //        [JsonProperty("displayName")]
        //        public string DisplayName { get; set; }

        //        [JsonProperty("billFlag")]
        //        public bool? BillFlag { get; set; }

        //        [JsonProperty("description")]
        //        public string Description { get; set; }

        //        [JsonProperty("neddAuth")]
        //        public bool? NeddAuth { get; set; }

        //        [JsonProperty("apiSystemParamVOList")]
        //        public ModelInfoResult[] ApiSystemParamVOList { get; set; }

        //        [JsonProperty("apiAppParamVOList")]
        //        public ModelInfoResult[] ApiAppParamVOList { get; set; }

        //        [JsonProperty("apiReturnParamVOList")]
        //        public ModelInfoResult[] ApiReturnParamVOList { get; set; }

        //        [JsonProperty("apiErrorCodeVOList")]
        //        public ApiErrorCodeVOList[] ApiErrorCodeVOList { get; set; }

        //        [JsonProperty("apiDocSampleVOList")]
        //        public ApiDocSampleVOList[] ApiDocSampleVOList { get; set; }

        //        [JsonProperty("returnExample")]
        //        public string ReturnExample { get; set; }
        //    }

        //    public class ModelInfoResult
        //    {

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("type")]
        //        public string Type { get; set; }

        //        [JsonProperty("required")]
        //        public bool? Required { get; set; }

        //        [JsonProperty("description")]
        //        public string Description { get; set; }

        //        [JsonProperty("exampleValue")]
        //        public string ExampleValue { get; set; }

        //        [JsonProperty("defaultValue")]
        //        public object DefaultValue { get; set; }

        //        [JsonProperty("complexTypeFlag")]
        //        public bool? ComplexTypeFlag { get; set; }

        //        [JsonProperty("typeName")]
        //        public string TypeName { get; set; }
        //    }


        //    public class Base2Response<T>
        //    {

        //        [JsonProperty("success")]
        //        public bool Success { get; set; }

        //        [JsonProperty("bizCode")]
        //        public string BizCode { get; set; }

        //        [JsonProperty("result")]
        //        public T Result { get; set; }

        //        [JsonProperty("errMsg")]
        //        public string ErrMsg { get; set; }
        //    }
        //}

        //namespace ddd
        //{
        //    public class TopicGroupsResult
        //    {

        //        [JsonProperty("id")]
        //        public string Id { get; set; }

        //        [JsonProperty("text")]
        //        public string Text { get; set; }
        //    }

        //    public class TopicsByGroupAndOwnerResult
        //    {

        //        [JsonProperty("topicId")]
        //        public string TopicId { get; set; }

        //        [JsonProperty("topicName")]
        //        public string TopicName { get; set; }

        //        [JsonProperty("topicDisplayName")]
        //        public string TopicDisplayName { get; set; }

        //        [JsonProperty("topicGroup")]
        //        public string TopicGroup { get; set; }

        //        [JsonProperty("topicGroupDisplayName")]
        //        public string TopicGroupDisplayName { get; set; }

        //        [JsonProperty("gmtModify")]
        //        public object GmtModify { get; set; }

        //        [JsonProperty("subscribed")]
        //        public bool? Subscribed { get; set; }



        //    }

        //    public class MessageDoc
        //    {

        //        [JsonProperty("name")]
        //        public string Name { get; set; }

        //        [JsonProperty("desc")]
        //        public string Desc { get; set; }

        //        [JsonProperty("type")]
        //        public string Type { get; set; }

        //        [JsonProperty("sample")]
        //        public string Sample { get; set; }

        //        [JsonProperty("required")]
        //        public bool Required { get; set; }

        //        [JsonProperty("children")]
        //        public MessageDoc[] Children { get; set; }

        //    }


        //    public class TopicResult
        //    {

        //        [JsonProperty("topicId")]
        //        public string TopicId { get; set; }

        //        [JsonProperty("topicName")]
        //        public string TopicName { get; set; }

        //        [JsonProperty("topicDisplayName")]
        //        public string TopicDisplayName { get; set; }

        //        [JsonProperty("topicGroupName")]
        //        public string TopicGroupName { get; set; }

        //        [JsonProperty("topicGroupDisplayName")]
        //        public string TopicGroupDisplayName { get; set; }

        //        [JsonProperty("desc")]
        //        public string Desc { get; set; }

        //        [JsonProperty("messageDocs")]
        //        public MessageDoc[] MessageDocs { get; set; }

        //        [JsonProperty("gmtModify")]
        //        public long GmtModify { get; set; }

        //        [JsonProperty("sample")]
        //        public string Sample { get; set; }

        //        [JsonProperty("subscribed")]
        //        public bool Subscribed { get; set; }
        //    }


    }
}



