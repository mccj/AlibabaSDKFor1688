using ConsoleApp2.sss;
using NJsonSchema;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class WebSocketDocument
    {
        public OpenApiDocument 创建文档(bool isFull)
        {
            //var document1 = OpenApiDocument.FromFileAsync(@"Data\swagger.json").Result;
            var document = 创建(isFull).GetAwaiter().GetResult();

            //keyValuePairs____.Sort();
            //var ddd = string.Join("\r\n", keyValuePairs____);

            return document;
        }
        private async Task<OpenApiDocument> 创建(bool isFull)
        {
            var document = new OpenApiDocument();

            Task<eeeeeeeee11>[] publicApiDetails;
            if (isFull)
            {
                var publicAllApis = await AlibabaDataCache.GetAllTopics();
                publicApiDetails = publicAllApis.Result.Select(async f => new eeeeeeeee11 { ApiInfo = f, ApiDetail = await AlibabaDataCache.GetTopic(f.TopicId) }).ToArray();
            }
            else
            {
                var apiCategorys = await AlibabaDataCache.GetTopicGroups();
                var aa = apiCategorys.Result.Select(async f => await AlibabaDataCache.GetTopicsByGroupAndOwner(f.Id));
                publicApiDetails = aa.SelectMany(f => f.Result.Result.Select(async ff => new eeeeeeeee11
                {
                    ApiInfo = ff,
                    ApiDetail = await AlibabaDataCache.GetTopic(ff.TopicId)
                })).ToArray();
            }


            for (int i = 0; i < publicApiDetails.Length; i++)
            {
                var publicApi = publicApiDetails[i].Result;
                Console.WriteLine((i + 1) + "." + publicApi.ApiInfo.TopicGroupDisplayName + ":" + publicApi.ApiInfo.TopicDisplayName);

                var apiDetail = publicApi.ApiDetail.Result;

                document.Definitions.Add(apiDetail.TopicId, getMessageTypeToSchema(apiDetail));

            }

            var rr = NJsonSchema.JsonSchema.FromType<string>().ToJsonSchemaProperty();
            rr.Title = "TypeDescription的json";
            rr.IsReadOnly = true;
            rr.Default = Newtonsoft.Json.JsonConvert.SerializeObject(publicApiDetails.ToDictionary(f => f.Result.ApiInfo.TopicId, f => f.Result.ApiInfo.TopicGroupDisplayName + "-" + f.Result.ApiInfo.TopicDisplayName));
            var rr1 = NJsonSchema.JsonSchema.FromType<string>().ToJsonSchemaProperty();
            rr1.Title = "TypeDescription的json";
            rr1.IsReadOnly = true;
            rr1.Default = Newtonsoft.Json.JsonConvert.SerializeObject(document.Definitions.ToDictionary(f => f.Key, f => f.Value.Id));

            var TypeDescriptionEnum = new NJsonSchema.JsonSchema { Type = JsonObjectType.String, Description = "消息类型" };
            document.Definitions.Add("TypeDescription", TypeDescriptionEnum);
            foreach (var item in publicApiDetails)
            {
                TypeDescriptionEnum.Enumeration.Add(item.Result.ApiInfo.TopicId);
                TypeDescriptionEnum.EnumerationNames.Add(item.Result.ApiInfo.TopicId);
            }

            var TypeDescriptionJson = new NJsonSchema.JsonSchema { Type = JsonObjectType.Object, AllowAdditionalProperties = false };
            document.Definitions.Add("TypeDescriptionJson", TypeDescriptionJson);
            TypeDescriptionJson.Properties.Add("JsonDescription", rr);
            TypeDescriptionJson.Properties.Add("JsonClass", rr1);
            return document;
        }


        private System.Collections.Generic.Dictionary<string, NJsonSchema.JsonSchema> keyValuePairstModelInfo = new System.Collections.Generic.Dictionary<string, NJsonSchema.JsonSchema>();
        private NJsonSchema.JsonSchema createJsonSchema(string type, ddd.MessageDoc[] modelInfoResult)
        {
            var _type = 获取数组维度(type);

            var ddd = getJsonSchemaCache(modelInfoResult, f =>
            {

                var jsonSchema = new NJsonSchema.JsonSchema
                {
                    AllowAdditionalProperties = false,
                    Type = NJsonSchema.JsonObjectType.Object
                };
                createJsonSchemaBy(jsonSchema, modelInfoResult);
                return jsonSchema;
            });
            return makeArraySchemaType(ddd, _type.arrLength);
        }
        private NJsonSchema.JsonSchema getJsonSchemaCache(ddd.MessageDoc[] modelInfoResult, Func<ddd.MessageDoc[], NJsonSchema.JsonSchema> func)
        {
            var md5key = string.Join("", modelInfoResult.Select(f => f.Name + f.Type));
            if (keyValuePairstModelInfo.ContainsKey(md5key))
            {
                var jsonSchema = keyValuePairstModelInfo[md5key];
                return jsonSchema;
            }
            else
            {
                var jsonSchema = func(modelInfoResult);
                keyValuePairstModelInfo.Add(md5key, jsonSchema);
                return jsonSchema;
            }
        }
        private void createJsonSchemaBy(NJsonSchema.JsonSchema jsonSchema, ddd.MessageDoc[] modelInfoResult)
        {
            if (modelInfoResult == null || modelInfoResult.Length == 0)
            {

            }
            else
            {
                foreach (var item in modelInfoResult)
                {
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                    }
                    else/* if(!jsonSchema.Properties.ContainsKey(item.Name))*/
                    {
                        jsonSchema.Properties.Add(item.Name, getJsonSchemaProperty(item.Name, item.Type, item.Required, item.Children, item.Desc?.过滤特殊字符()));
                    }
                }
            }
        }
        private NJsonSchema.JsonSchemaProperty getJsonSchemaProperty(string name, string type, bool required, ddd.MessageDoc[] children, string description)
        {
            var tt = children == null ? getSystemTypeToSchema(type) : createJsonSchema(type, children); //getSchema(document, @namespace, apiname, version, type, "");
            var r = tt.ToJsonSchemaProperty();
            r.AllowAdditionalProperties = false;
            r.Description = description;
            r.IsRequired = required;
            //Default = defaultValue

            return r;
        }


        #region 类型转换
        private NJsonSchema.JsonSchema getMessageTypeToSchema(ddd.TopicResult apiDetail)
        {
            var key = apiDetail.TopicId;
            if (keyValuePairstModelInfo.ContainsKey(key))
            {
                return keyValuePairstModelInfo[key];
            }
            else
            {
                var modelInfoResult = apiDetail.MessageDocs;
                var md5key = string.Join("", modelInfoResult?.Select(f => f.Name + f.Type).ToArray() ?? new string[] { });
                if (keyValuePairstModelInfo.ContainsKey(md5key))
                {
                    return keyValuePairstModelInfo[md5key];
                }
                else
                {
                    var jsonSchema = new NJsonSchema.JsonSchema
                    {
                        Id = key,
                        AllowAdditionalProperties = false,
                        Type = NJsonSchema.JsonObjectType.Object,
                        Description = $"{apiDetail.TopicGroupDisplayName}-{apiDetail.TopicDisplayName}\r\nhttps://open.1688.com/doc/topicDetail.htm?id={apiDetail.TopicId}&topicGroup={apiDetail.TopicGroupName}"
                    };
                    keyValuePairstModelInfo.Add(key, jsonSchema);
                    keyValuePairstModelInfo.Add(md5key, jsonSchema);
                    createJsonSchemaBy(jsonSchema, modelInfoResult);

                    return jsonSchema;
                }


                //var rr = NJsonSchema.JsonSchema.FromType<string>().ToJsonSchemaProperty();
                //rr.Title = "指当前类型的具体功能";
                //rr.IsReadOnly = true;
                //rr.Default = $"{apiDetail.TopicGroupDisplayName}-{apiDetail.TopicDisplayName}";
                //jsonSchema.Properties.Add("TypeDescription", rr);

            }
        }

        private NJsonSchema.JsonSchema getSystemTypeToSchema(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;

            var _type = 获取数组维度(type);
            switch (_type.type?.ToLower())
            {
                //case "int":
                //case "integer":
                //case "java.lang.integer":
                //return makeArraySystemType(typeof(int), _type.arrLength);
                //case "java.lang.float":
                //    return makeArraySystemType(typeof(float), _type.arrLength);
                case "number":
                    //case "long":
                    //case "java.lang.long":
                    return makeArraySystemType(typeof(long), _type.arrLength);
                //case "java.lang.double":
                //case "double":
                //    return makeArraySystemType(typeof(double), _type.arrLength);
                //case "bigdecimal":
                //case "java.math.bigdecimal":
                //    return makeArraySystemType(typeof(decimal), _type.arrLength);
                case "boolean":
                    //case "java.lang.boolean":
                    return makeArraySystemType(typeof(bool), _type.arrLength);
                case "date":
                    //case "java.util.date":
                    return makeArraySystemType(typeof(DateTime), _type.arrLength);
                //case "byte":
                //case "java.lang.byte":
                //case "b":
                //    return makeArraySystemType(typeof(byte), _type.arrLength);
                case "string":
                    //case "java.lang.string":
                    //case "ljava.lang.string":
                    return makeArraySystemType(typeof(string), _type.arrLength);
                //case "inputstream":
                //case "java.io.inputstream":
                //    return makeArraySystemType(typeof(byte[]), _type.arrLength);
                //case "t":
                //case "object":
                //case "java.lang.object":
                //case "com.alibaba.fastjson.jsonobject":
                //    return makeArraySystemType(typeof(object), _type.arrLength);
                //case "java.util.list":
                //case "list":
                //case "java.util.set":
                //    return makeArraySystemType(typeof(object[]), _type.arrLength);
                //case "map":
                //case "java.util.map":
                //    return makeArraySchemaType(DictionarySchema(typeof(object)), _type.arrLength);
                //case "java.lang.throwable":
                //case "java.lang.void":
                //    return makeArraySystemType(typeof(object), _type.arrLength);


                default:
                    return makeArraySystemType(typeof(object), 0);
            }
        }
        private (string type, int arrLength) 获取数组维度(string type)
        {
            //type = type?.ToLower();
            var arrLength = 0;
            for (int i = 0; i < 100; i++)
            {
                if (type.EndsWith("[]"))
                {
                    arrLength++;
                    type = type.Remove(type.Length - 2);
                }
                else if (type.StartsWith("["))
                {
                    arrLength++;
                    type = type.Remove(0, 1).Trim(';');
                }
                else
                {
                    break;
                }
            }
            //var byteArr = new[] { "byte", "java.lang.Byte", "b" };
            //if (byteArr.Contains(type) && arrLength > 0)
            //{
            //    type = "byte[]";
            //    arrLength -= 1;
            //}
            return (type: type, arrLength: arrLength);
        }
        private NJsonSchema.JsonSchema makeArraySystemType(Type type, int arrLength)
        {
            if (type != null)
            {
                for (int i = 0; i < arrLength; i++)
                {
                    type = type.MakeArrayType();
                }

                return NJsonSchema.JsonSchema.FromType(type);
            }
            else
            {
                return null;
            }
        }
        private NJsonSchema.JsonSchema makeArraySchemaType(NJsonSchema.JsonSchema jsonSchema, int arrLength)
        {
            if (jsonSchema != null)
            {
                for (int i = 0; i < arrLength; i++)
                {
                    jsonSchema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.Array, Item = jsonSchema };
                }

                return jsonSchema;
            }
            else
            {
                return null;
            }
        }
        #endregion 类型转换

        #region ToCode
        public string ToCSharpCode(OpenApiDocument document, string _namespace)
        {
            var settings = new CSharpClientGeneratorSettings
            {
                CSharpGeneratorSettings =
                {
                    Namespace = _namespace,//"AlibabaSDK"
                },
                //GenerateSyncMethods = true,
                ////GenerateDtoTypes = false,
                //GenerateClientInterfaces = true,
                //GenerateExceptionClasses = false,
                //GenerateOptionalParameters = true,
            };
            //settings.CodeGeneratorSettings.TemplateDirectory = "";
            settings.CodeGeneratorSettings.GenerateDefaultValues = true;
            settings.CodeGeneratorSettings.PropertyNameGenerator = new MyCSharpPropertyNameGenerator();
            settings.CSharpGeneratorSettings.GenerateJsonMethods = true;

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            return code;

        }
        public string ToTypeScriptCode(OpenApiDocument document)
        {
            //var document = OpenApiDocument.FromUrl(swaggerSpecUrl);

            var settings = new TypeScriptClientGeneratorSettings
            {
                ClassName = "{controller}Client",
            };

            var generator = new TypeScriptClientGenerator(document, settings);
            var code = generator.GenerateFile();
            return code;
        }
        #endregion ToCode
    }

    public class eeeeeeeee11
    {
        public ddd.TopicsByGroupAndOwnerResult ApiInfo { get; set; }
        public Base2Response<ddd.TopicResult> ApiDetail { get; set; }
    }

}
