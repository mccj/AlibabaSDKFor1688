using ConsoleApp2.Taobao.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ConsoleApp2.Aliexpress
{

    public class AliexpressApi
    {
        private System.Net.Http.HttpClient _httpClient;
        public AliexpressApi()
        {
            _httpClient = new HttpClient();
            //_httpClient.DefaultRequestHeaders.Add("Cookie", "_tb_token_=e734013eb3593");
        }
        public async Task<CatelogTree[]> GetCatelogConfig()
        {
            var response = await _httpClient.GetAsync("https://developers.aliexpress.com/handler/document/getCatelogConfig.json?isEn=false");
            var json = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
            //var r = result?.Data?.TreeCategories?.SelectMany(f => f.CatelogTrees).ToArray();
            //return r;
            //var dd = result["data"]["treeCategories"][2]["catelogTrees"][0]["catelogList"];
            var dd = result.SelectTokens("data.treeCategories[?(@.name=='API文档')].catelogTrees[?(@.name=='API文档')].catelogList[?(@.docType==0)]");//["data"]["treeCategories"][2]["catelogTrees"][0]["catelogList"];
            return null;
        }
        public async Task<DocumentData> GetDocument(int docId, int docType)
        {
            var response = await _httpClient.GetAsync($"https://developers.aliexpress.com/handler/document/getDocument.json?isEn=false&docId={docId}&docType={docType}");
            var json = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<DocumentData>>(json);
            //var r= result?.Data?.TreeCategories?.SelectMany(f=>f.CatelogTrees).ToArray();
            return result?.Data;
        }
    }
}