using ConsoleApp2.sss;
using System;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class AlibabaDataCache
    {
        private readonly static AlibabaApi alibabaApi = new AlibabaApi();
        public static async Task<Base2Response<CategoryResult[]>> GetAllApiCategoryAsync()
        {
            var path = @"Data\缓存\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, "AllApiCategorys.json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<CategoryResult[]>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.GetAllApiCategory();
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }

        public static async Task<BaseResponse<Datum[]>> ListPublicApiByCacheAsync()
        {
            var path = @"Data\缓存\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, "PublicApis.json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<Datum[]>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.ListPublicApiAsync();
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }

        public static async Task<BaseResponse<ArgumentData>> GetApiArgumentsByCacheAsync(string @namespace, string name, int version)
        {
            var path = @"Data\缓存\ApiArguments\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, @namespace + "_" + name + "_" + version + ".json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<ArgumentData>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.GetApiArguments(@namespace, name, version);
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }


        public static async Task<Base2Response<ListByCategoryResult[]>> GetAopApiListByCategoryByCacheAsync(string aopApiCategory)
        {
            var path = @"Data\缓存\ApiCategory\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, aopApiCategory + ".json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<ListByCategoryResult[]>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.GetAopApiListByCategory(aopApiCategory);
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }
        public static async Task<Base2Response<DetailResult>> GetApiDetailByCacheAsync(string @namespace, string name, int version)
        {
            try
            {
                var path = @"Data\缓存\ApiDetail\";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                var f = System.IO.Path.Combine(path, @namespace + "_" + name + "_" + version + ".json");
                if (System.IO.File.Exists(f))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<DetailResult>>(System.IO.File.ReadAllText(f));
                var sssddd = await alibabaApi.GetApiDetail(@namespace, name, version);
                System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
                return sssddd;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
        }
        public static async Task<Base2Response<ModelInfoResult[]>> GetModelInfoByCacheAsync(string @namespace, string apiname, int version, string typeName)
        {
            try
            {
                var path = @"Data\缓存\ModelInfo\";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                var f = System.IO.Path.Combine(path, @namespace + "_" + apiname + "_" + version + "_" + typeName + ".json");
                if (System.IO.File.Exists(f))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<ModelInfoResult[]>>(System.IO.File.ReadAllText(f));
                var sssddd = await alibabaApi.GetModelInfo(@namespace, apiname, version, typeName);
                System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
                return sssddd;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
        }

        public static async Task<Datum[]> GetSolutionApiAndMessageListDetailByCacheAsync()
        {
            try
            {
                var path = @"Data\缓存\";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                var f = System.IO.Path.Combine(path, "SolutionApiAndMessageListDetail.json");
                if (System.IO.File.Exists(f))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Datum[]>(System.IO.File.ReadAllText(f));
                var sssddd = await alibabaApi.GetSolutionApiAndMessageListDetail();
                System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
                return sssddd;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
        }










        public static async Task<Base2Response<ddd.TopicGroupsResult[]>> GetTopicGroups()
        {
            var path = @"Data\缓存\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, "GetTopicGroups.json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<ddd.TopicGroupsResult[]>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.GetTopicGroups();
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }
        public static async Task<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>> GetTopicsByGroupAndOwner(string topicGroup)
        {
            var path = @"Data\缓存\TopicsByGroupAndOwner\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, "GetTopicsByGroupAndOwner_" + topicGroup + ".json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.GetTopicsByGroupAndOwner(topicGroup);
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }
        public static async Task<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>> GetAllTopics()
        {
            var path = @"Data\缓存\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, "GetAllTopics.json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<ddd.TopicsByGroupAndOwnerResult[]>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.GetAllTopics();
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }
        public static async Task<Base2Response<ddd.TopicResult>> GetTopic(string topicId)
        {
            var path = @"Data\缓存\Topic\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            var f = System.IO.Path.Combine(path, "GetTopic_" + topicId + ".json");
            if (System.IO.File.Exists(f))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Base2Response<ddd.TopicResult>>(System.IO.File.ReadAllText(f));
            var sssddd = await alibabaApi.GetTopic(topicId);
            System.IO.File.WriteAllText(f, Newtonsoft.Json.JsonConvert.SerializeObject(sssddd));
            return sssddd;
        }
    }
}