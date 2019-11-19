using ConsoleApp2.Alibaba.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ConsoleApp2.Alibaba
{

    public class AlibabaApi
    {
        private System.Net.Http.HttpClient _httpClient;
        public AlibabaApi()
        {
            _httpClient = new HttpClient();
        }
        public async Task<BaseResponse<System.DateTime>> GetSystemTimestamp()
        {
            var response = await _httpClient.GetAsync("https://gw.open.1688.com/console/service/getSystemTimestamp.json");
            var result = await response.Content.ReadAsAsync<BaseResponse<System.DateTime>>();
            return result;
        }
        public async Task<BaseResponse<Datum[]>> ListPublicApiAsync()
        {
            var response = await _httpClient.GetAsync("https://gw.open.1688.com/console/service/listPublicApi.json");
            var result = await response.Content.ReadAsAsync<BaseResponse<Datum[]>>();
            return result;
        }
        public async Task<BaseResponse<ArgumentData>> GetApiArguments(string @namespace, string apiname, int version)
        {
            var response = await _httpClient.PostAsJsonAsync("https://gw.open.1688.com/console/service/getApiArguments.json", new { @namespace, apiname, version });
            var result = await response.Content.ReadAsAsync<BaseResponse<ArgumentData>>();
            return result;
        }
        public async Task<Base2Response<CategoryResult[]>> GetAllApiCategory()
        {
            var response = await _httpClient.GetAsync("https://open.1688.com/api/data/getAllApiCategory.json");
            var result = await response.Content.ReadAsAsync<Base2Response<CategoryResult[]>>();
            return result;
        }
        public async Task<Base2Response<ListByCategoryResult[]>> GetAopApiListByCategory(string aopApiCategory)
        {
            var response = await _httpClient.GetAsync("https://open.1688.com/api/data/getAopApiListByCategory.json?aopApiCategory=" + aopApiCategory);
            var result = await response.Content.ReadAsAsync<Base2Response<ListByCategoryResult[]>>();
            return result;
        }
        public async Task<Base2Response<DetailResult>> GetApiDetail(string @namespace, string apiname, int version)
        {
            var response = await _httpClient.GetAsync($"https://open.1688.com/api/data/getApiDetail.json?_input_charset=UTF-8&namespace={@namespace}&name={apiname}&version={version}");
            var result = await response.Content.ReadAsAsync<Base2Response<DetailResult>>();
            return result;
        }
        public async Task<Base2Response<ModelInfoResult[]>> GetModelInfo(string @namespace, string apiname, int version, string typeName)
        {
            var response = await _httpClient.GetAsync($"https://open.1688.com/api/data/getModelInfo.json?_input_charset=UTF-8&type=2&namespace={@namespace}&apiname={apiname}&version={version}&typeName={typeName}");
            //var result1 = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<Base2Response<ModelInfoResult[]>>();
            return result;
        }



        public async Task<Base2Response<ddd.TopicGroupsResult[]>> GetTopicGroups()
        {
            var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getTopicGroups.json?_input_charset=UTF-8");
            var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicGroupsResult[]>>();
            return result;
        }
        public async Task<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>> GetTopicsByGroupAndOwner(string topicGroup)
        {
            var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getTopicsByGroupAndOwner.json?_input_charset=UTF-8&topicGroup=" + topicGroup);
            var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>>();
            return result;
        }
        public async Task<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>> GetAllTopics()
        {
            var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getAllTopics.json?_input_charset=UTF-8");
            var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>>();
            return result;
        }
        public async Task<Base2Response<ddd.TopicResult>> GetTopic(string topicId)
        {
            var response = await _httpClient.GetAsync("https://open.1688.com/msg/dataNew/getTopic.json?_input_charset=UTF-8&topicId=" + topicId);
            var result = await response.Content.ReadAsAsync<Base2Response<ddd.TopicResult>>();
            return result;
        }


        public async Task<Datum[]> GetSolutionApiAndMessageListDetail()
        {
            var response = await _httpClient.GetAsync($"https://open.1688.com/solution/data/getSolutionDetail.jsonp?solutionKey=1513248184893&callback=jsonp");
            var result1 = await response.Content.ReadAsStringAsync();
            var ss = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(result1.Remove(0, 5).Trim('(', ')'));
            var ddjson = ss.SelectToken("$.result.solutionDescList[?(@subCategory=='apiAndMessageList')].content").ToObject<string>();
            var ss1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(ddjson);
            var ss2 = ss1.SelectTokens("$.apis[*].modules[*]");
            var result = ss2.Select(f => new Datum
            {
                //description = f.Value<string>("description"),
                //fullName = f.Value<string>("fullName"),
                Name = f.Value<string>("name"),
                Namespace = f.Value<string>("namespace"),
                Version = f.Value<int>("version")
            }).ToArray();
            return result;
        }
    }

    namespace Models
    {
        public class BaseResponse<T>
        {

            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("data")]
            public T Data { get; set; }
        }

        public class Datum
        {

            [JsonProperty("namespace")]
            public string Namespace { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public int Version { get; set; }
        }
        public class Field
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("required")]
            public bool Required { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("sampleValue")]
            public string SampleValue { get; set; }
        }
        public class Domain
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("fields")]
            public Field[] Fields { get; set; }
        }
        public class Param
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("required")]
            public bool Required { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("defaultValue")]
            public string DefaultValue { get; set; }

            [JsonProperty("sampleValue")]
            public string SampleValue { get; set; }
        }
        public class ArgumentData
        {

            [JsonProperty("domains")]
            public System.Collections.Generic.Dictionary<string, Domain> Domains { get; set; }

            [JsonProperty("params")]
            public Param[] Params { get; set; }
        }

        public class CategoryResult
        {

            [JsonProperty("aopCategoryCode")]
            public string AopCategoryCode { get; set; }

            [JsonProperty("aopCategoryName")]
            public string AopCategoryName { get; set; }
        }


        public class ListByCategoryResult
        {

            [JsonProperty("oceanApiId")]
            public string OceanApiId { get; set; }

            [JsonProperty("namespace")]
            public string Namespace { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public int Version { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("gmtModified")]
            public object GmtModified { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("aopCategoryCode")]
            public string AopCategoryCode { get; set; }
        }


        public class ApiErrorCodeVOList
        {

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("desc")]
            public string Desc { get; set; }

            [JsonProperty("howToFix")]
            public string HowToFix { get; set; }
        }

        public class ApiDocSampleVOList
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("sample")]
            public string Sample { get; set; }

            [JsonProperty("type")]
            public object Type { get; set; }
        }
        public class DetailResult
        {

            [JsonProperty("namespace")]
            public string Namespace { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public int Version { get; set; }

            [JsonProperty("oceanApiId")]
            public string OceanApiId { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("billFlag")]
            public bool? BillFlag { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("neddAuth")]
            public bool? NeddAuth { get; set; }

            [JsonProperty("apiSystemParamVOList")]
            public ModelInfoResult[] ApiSystemParamVOList { get; set; }

            [JsonProperty("apiAppParamVOList")]
            public ModelInfoResult[] ApiAppParamVOList { get; set; }

            [JsonProperty("apiReturnParamVOList")]
            public ModelInfoResult[] ApiReturnParamVOList { get; set; }

            [JsonProperty("apiErrorCodeVOList")]
            public ApiErrorCodeVOList[] ApiErrorCodeVOList { get; set; }

            [JsonProperty("apiDocSampleVOList")]
            public ApiDocSampleVOList[] ApiDocSampleVOList { get; set; }

            [JsonProperty("returnExample")]
            public string ReturnExample { get; set; }
        }

        public class ModelInfoResult
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("required")]
            public bool? Required { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("exampleValue")]
            public string ExampleValue { get; set; }

            [JsonProperty("defaultValue")]
            public object DefaultValue { get; set; }

            [JsonProperty("complexTypeFlag")]
            public bool? ComplexTypeFlag { get; set; }

            [JsonProperty("typeName")]
            public string TypeName { get; set; }
        }


        public class Base2Response<T>
        {

            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("bizCode")]
            public string BizCode { get; set; }

            [JsonProperty("result")]
            public T Result { get; set; }

            [JsonProperty("errMsg")]
            public string ErrMsg { get; set; }
        }
    }

    namespace ddd
    {
        public class TopicGroupsResult
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }
        }

        public class TopicsByGroupAndOwnerResult
        {

            [JsonProperty("topicId")]
            public string TopicId { get; set; }

            [JsonProperty("topicName")]
            public string TopicName { get; set; }

            [JsonProperty("topicDisplayName")]
            public string TopicDisplayName { get; set; }

            [JsonProperty("topicGroup")]
            public string TopicGroup { get; set; }

            [JsonProperty("topicGroupDisplayName")]
            public string TopicGroupDisplayName { get; set; }

            [JsonProperty("gmtModify")]
            public object GmtModify { get; set; }

            [JsonProperty("subscribed")]
            public bool? Subscribed { get; set; }



        }

        public class MessageDoc
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("desc")]
            public string Desc { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("sample")]
            public string Sample { get; set; }

            [JsonProperty("required")]
            public bool Required { get; set; }

            [JsonProperty("children")]
            public MessageDoc[] Children { get; set; }

        }


        public class TopicResult
        {

            [JsonProperty("topicId")]
            public string TopicId { get; set; }

            [JsonProperty("topicName")]
            public string TopicName { get; set; }

            [JsonProperty("topicDisplayName")]
            public string TopicDisplayName { get; set; }

            [JsonProperty("topicGroupName")]
            public string TopicGroupName { get; set; }

            [JsonProperty("topicGroupDisplayName")]
            public string TopicGroupDisplayName { get; set; }

            [JsonProperty("desc")]
            public string Desc { get; set; }

            [JsonProperty("messageDocs")]
            public MessageDoc[] MessageDocs { get; set; }

            [JsonProperty("gmtModify")]
            public long GmtModify { get; set; }

            [JsonProperty("sample")]
            public string Sample { get; set; }

            [JsonProperty("subscribed")]
            public bool Subscribed { get; set; }
        }


    }
}

