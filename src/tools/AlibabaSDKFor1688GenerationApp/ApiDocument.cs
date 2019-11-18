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
    public class ApiDocument
    {
        public OpenApiDocument 创建文档(bool isFull)
        {
            //var document1 = OpenApiDocument.FromFileAsync(@"Data\swagger.json").Result;
            var document = 创建(isFull).GetAwaiter().GetResult();

            keyValuePairs____.Sort();
            var ddd = string.Join("\r\n", keyValuePairs____);

            return document;
        }
        private async Task<OpenApiDocument> 创建(bool isFull)
        {
            var document = new OpenApiDocument();

            document.Servers.Add(new OpenApiServer { Url = "https://gw.open.1688.com/" });
            System.Collections.Generic.List<Task<eeeeeeeee>> publicApiDetails;
            if (isFull)
            {
                var publicAllApis = await AlibabaDataCache.ListPublicApiByCacheAsync();
                var publicApis = publicAllApis.Data.GroupBy(f => new { f.Namespace, f.Name }).Select(f => new Datum { Name = f.Key.Name, Namespace = f.Key.Namespace, Version = f.Max(ff => ff.Version) }).ToArray();
                publicApiDetails = publicApis.Select(async f => new eeeeeeeee { ApiInfo = f, ApiDetail = await AlibabaDataCache.GetApiDetailByCacheAsync(f.Namespace, f.Name, f.Version) })/*.Skip(587).Take(1)*/.ToList();
            }
            else
            {
                var apiCategorys = await AlibabaDataCache.GetAllApiCategoryAsync();
                var aa = apiCategorys.Result.Select(async f => await AlibabaDataCache.GetAopApiListByCategoryByCacheAsync(f.AopCategoryCode));
                publicApiDetails = aa.SelectMany(f => f.Result.Result.Select(async ff => new eeeeeeeee
                {
                    ApiInfo = new Datum { Name = ff.Name, Namespace = ff.Name, Version = ff.Version },
                    ApiDetail = await AlibabaDataCache.GetApiDetailByCacheAsync(ff.Namespace, ff.Name, ff.Version)
                })).ToList();
            }

            var 另外添加 = new[] {
                                //创建订单前预览数据接口
                new Datum { Namespace = "com.alibaba.trade", Name = "alibaba.createOrder.previewWithOBUid", Version = 1 },
                //跨境订单创建
                new Datum { Namespace = "com.alibaba.trade", Name = "alibaba.trade.createCrossOrder", Version = 1 },
                //查询订单可以支持的支付渠道
                new Datum { Namespace = "com.alibaba.trade", Name = "alibaba.trade.payWay.query", Version = 1 },
                //批量获取订单的支付链接
                new Datum { Namespace = "com.alibaba.trade", Name = "alibaba.alipay.url.get", Version = 1 },
                //获取使用跨境宝支付的支付链接
                new Datum { Namespace = "com.alibaba.trade", Name = "alibaba.crossBorderPay.url.get", Version = 1 },
                //获取使用诚e赊支付的支付链接
                new Datum { Namespace = "com.alibaba.trade", Name = "alibaba.creditPay.url.get", Version = 1 },
                //跨境场景获取商品详情
                new Datum { Namespace = "com.alibaba.product", Name = "alibaba.cross.productInfo", Version = 1 },
                //跨境场景下将商品加入铺货列表
                new Datum { Namespace = "com.alibaba.product.push", Name = "alibaba.cross.syncProductListPushed", Version = 1 },
                //同步铺货结果
                new Datum { Namespace = "com.alibaba.product.push", Name = "alibaba.product.push.syncPushProductResult", Version = 1 }
  };

            publicApiDetails.AddRange(
                另外添加.Select(async f => new eeeeeeeee { ApiInfo = f, ApiDetail = await AlibabaDataCache.GetApiDetailByCacheAsync(f.Namespace, f.Name, f.Version) })
            );

            //publicApiDetails = publicApiDetails.Skip(15).Take(1).ToArray();
            var errorSchema = errorJsonSchemaResponse();
            document.Definitions.Add("ErrorResponse", errorSchema);
            document.Security = new[] { new OpenApiSecurityRequirement() { { "NeddAuth", new[] { "access_token" } } } };

            for (int i = 0; i < publicApiDetails.Count; i++)
            {
                var publicApi = publicApiDetails[i].Result;
                Console.WriteLine((i + 1) + "." + publicApi.ApiInfo.Namespace + ":" + publicApi.ApiInfo.Name + "-" + publicApi.ApiInfo.Version);

                var apiDetail = publicApi.ApiDetail.Result;
                //var apiArguments = await GetApiArguments(item.Namespace, item.Name, item.Version);
                if (apiDetail == null)
                {

                }
                var description = ($" \r\n{apiDetail.DisplayName}\r\n{apiDetail.Description}\r\n\r\n文档: https://open.1688.com/api/apidocdetail.htm?id={apiDetail.OceanApiId} \r\n调试:https://open.1688.com/api/apiTool.htm?ns={apiDetail.Namespace}&n={apiDetail.Name}&v={apiDetail.Version} \r\n ")?.过滤特殊字符();
                var openApiOperation = new OpenApiOperation
                {
                    Summary = description,
                    //Description = item.Description,
                    OperationId = ((isFull ? (apiDetail.Namespace + ".") : "") + apiDetail.Name)?.ToPascalCase(),
                    Tags = {
                        "namespace=" + apiDetail.Namespace,
                        "name=" + apiDetail.Name,
                        "version=" + apiDetail.Version,
                        "oceanApiId=" + apiDetail.OceanApiId,
                        "billFlag=" + apiDetail.BillFlag,
                        "neddAuth=" + apiDetail.NeddAuth
                    },
                    Responses = { { "400", new OpenApiResponse { Schema = new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = errorSchema } } } }
                };

                //if (openApiOperation.OperationId == "DaeServiceRevoke4Aboss")
                //{
                //}
                //else
                {
                    if (apiDetail.NeddAuth == true)
                        openApiOperation.Security = document.Security;


                    //foreach (var item in apiDetail.ApiSystemParamVOList)
                    //{
                    //    openApiOperation.RequestBody = new OpenApiRequestBody { Content = { { item.Name, new OpenApiMediaType { Schema = JsonSchema.FromType<string>() } } } };
                    //}
                    foreach (var item in apiDetail.ApiAppParamVOList.OrderByDescending(f => f.Required))
                    {
                        openApiOperation.Parameters.Add(new OpenApiParameter
                        {
                            Kind = OpenApiParameterKind.FormData,
                            Name = item.Name,
                            IsRequired = item.Required == true,
                            Default = item.DefaultValue,
                            Description = item.Description?.过滤特殊字符(),
                            Schema = getSchema(document.Definitions, apiDetail.Namespace, apiDetail.Name, apiDetail.Version, item.Name, item.Type, item.Description?.过滤特殊字符())
                        });
                    }
                    var apiErrorCodeDescription = "";
                    if (apiDetail.ApiErrorCodeVOList.Any())
                    {
                        apiErrorCodeDescription = "返回 ErrorCode 的错误信息\r\n" +
                            string.Join("\r\n", apiDetail.ApiErrorCodeVOList.Select(f => f.Code + "\t- " + f.Desc + (string.IsNullOrWhiteSpace(f.HowToFix) ? "" : ("(" + f.HowToFix + ")"))));
                    }

                    var _apiReturnParamVOList = apiDetail.ApiReturnParamVOList.Where(f => !string.IsNullOrWhiteSpace(f.Name)).ToArray();
                    if (_apiReturnParamVOList.Length <= 1)
                    {
                        var item = _apiReturnParamVOList.SingleOrDefault();
                        var openApiResponse = new OpenApiResponse
                        {
                            Description = item?.Description?.过滤特殊字符() + apiErrorCodeDescription,
                        };
                        if (item != null)
                            //openApiResponse.Content.Add("application/json", new OpenApiMediaType { Schema = getSchema(document, apiDetail.Namespace, apiDetail.Name, apiDetail.Version, item.Type, item.Description) });
                            openApiResponse.Schema = getSchemaResponse(document.Definitions, apiDetail, item);
                        openApiOperation.Responses.Add("200", openApiResponse);
                    }
                    else
                    {
                        var jsonSchema = createJsonSchema(apiDetail.Namespace, apiDetail.Name, apiDetail.Version, _apiReturnParamVOList);
                        jsonSchema.Description = description;

                        var typeName = openApiOperation.OperationId + "Result";
                        if (!document.Definitions.ContainsKey(typeName))
                            document.Definitions.Add(typeName, jsonSchema);
                        else
                        {
                            document.Definitions.Add(apiDetail.Namespace?.ToPascalCase() + typeName, jsonSchema);

                            //var dfsdfsd = document.Definitions[jjjjj];
                        }
                        var openApiResponse = new OpenApiResponse
                        {
                            Description = apiErrorCodeDescription,
                            Schema = new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = jsonSchema }
                        };
                        //openApiResponse.Content.Add("application/json", new OpenApiMediaType { Schema = new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = jsonSchema } });
                        openApiOperation.Responses.Add("200", openApiResponse);
                    }


                    foreach (var item2 in apiDetail.ApiDocSampleVOList)
                    {

                    }


                    document.Paths.Add($"/openapi/param2/{apiDetail.Version}/{apiDetail.Namespace}/{apiDetail.Name}/{{AppKey}}", new OpenApiPathItem
                    {
                        { OpenApiOperationMethod.Post,openApiOperation}
                    });
                }
            }

            return document;
        }
        private NJsonSchema.JsonSchema getSchemaResponse(System.Collections.Generic.IDictionary<string, JsonSchema> definitions, DetailResult apiDetail, ModelInfoResult item)
        {
            var ss = getSchema(definitions, apiDetail.Namespace, apiDetail.Name, apiDetail.Version, item.Name, item.Type, item.Description);

            var 排除OceanApiId = new[] {
                "cn.alibaba.open:system.time.get-1"
            };
            var 包含OceanApiId = new[] {
                "cn.alibaba.open:push.query.messageList-1",
                "cn.alibaba.open:push.cursor.messageList-1",
                "com.alibaba.product.push:alibaba.cross.syncProductListPushed-1"
  };
            if (!排除OceanApiId.Contains(apiDetail.OceanApiId) && (ss.Reference == null || 包含OceanApiId.Contains(apiDetail.OceanApiId)))
            {
                var ssssss = new NJsonSchema.JsonSchema
                {
                    AllowAdditionalProperties = false,
                    Description = item.Description,
                    Properties =
                    {
                        { item.Name?.类名过滤特殊字符(), ss.ToJsonSchemaProperty() }
                    }
                };

                var key = string.Join("", ssssss.Properties.Select(f => f.Key + f.Value.Type + f.Value.Title));
                if (keyValuePairstModelInfo.ContainsKey(key))
                {
                    return keyValuePairstModelInfo[key];
                }
                else
                {
                    keyValuePairstModelInfo.Add(key, ssssss);
                    return ssssss;
                }

            }
            else
            {
                return ss;
            }

        }
        private NJsonSchema.JsonSchema errorJsonSchemaResponse()
        {
            var typeString = NJsonSchema.JsonSchema.FromType(typeof(string));

            var jsonSchema = new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Description = "返回错误信息" };
            jsonSchema.Properties.Add("error_message", typeString.ToJsonSchemaProperty(f => { f.Description = "错误信息"; }));
            jsonSchema.Properties.Add("exception", typeString.ToJsonSchemaProperty(f => f.Description = "异常描述"));
            jsonSchema.Properties.Add("error_code", typeString.ToJsonSchemaProperty());
            jsonSchema.Properties.Add("request_id", typeString.ToJsonSchemaProperty(f => f.Description = "请求标识"));
            return jsonSchema;
        }
        private System.Collections.Generic.List<string> keyValuePairs____ = new System.Collections.Generic.List<string>();
        private NJsonSchema.JsonSchema getSchema(System.Collections.Generic.IDictionary<string, JsonSchema> definitions, string @namespace, string apiname, int version, string name, string type, string description)
        {
            var key = @namespace + ":" + apiname + "-" + version + "=>" + type;
            var jsonSchema = keyValuePairs.GetOrAdd(key, t =>
             {
                 var jsonSchema1 = eeee(definitions, key, @namespace, apiname, version, name, type, description) ?? getMessageTypeToSchema(definitions, @namespace, apiname, version, type, description) ?? getSystemTypeToSchema(type);

                 return jsonSchema1;
             });

            if (jsonSchema == null)
            {
                keyValuePairs____.Add(type + "\t\t\t=\t\t\t" + @namespace + ":" + apiname + "-" + version);
                //TODO:eeeeeeee
                return NJsonSchema.JsonSchema.FromType<object>();
            }
            return jsonSchema;
        }
        private NJsonSchema.JsonSchema eeee(System.Collections.Generic.IDictionary<string, JsonSchema> definitions, string key, string @namespace, string apiname, int version, string name, string type, string description)
        {
            switch (key)
            {
                case "aliexpress.open:api.getChildrenPostCategoryById-1=>":
                    return getAliexpressOpenApiGetChildrenPostCategoryByIdV1(definitions);
                //case "cn.alibaba.open:push.query.messageList-1=>message:PushMessagePage":
                ////return getCnAlibabaOpenPushMessageListV1(definitions, "pushMessagePage", @namespace, apiname, version, type, description);
                //case "cn.alibaba.open:push.cursor.messageList-1=>message:PushMessage[]":
                //    //return getCnAlibabaOpenPushMessageListV1(definitions, "pushMessageList", @namespace, apiname, version, type, description);
                //    return getCnAlibabaOpenPushMessageListV1(definitions, name, @namespace, apiname, version, type, description);
                default:
                    break;
            }
            if (key.StartsWith("cn.alibaba.open:push.cursor.messageList-1"))
            {

            }

            return null;
        }
        ///// <summary>
        ///// https://open.1688.com/api/apidocdetail.htm?id=cn.alibaba.open:push.query.messageList-1
        ///// https://open.1688.com/api/apidocdetail.htm?id=cn.alibaba.open:push.cursor.messageList-1 
        ///// </summary>

        //private NJsonSchema.JsonSchema getCnAlibabaOpenPushMessageListV1(System.Collections.Generic.IDictionary<string, JsonSchema> definitions, string propertieName, string @namespace, string apiname, int version, string type, string description)
        //{
        //    var jsonSchema = getMessageTypeToSchema(definitions, @namespace, apiname, version, type, description);


        //    var pushMessageList = new NJsonSchema.JsonSchema
        //    {
        //        AllowAdditionalProperties = false,
        //        Type = NJsonSchema.JsonObjectType.Object,
        //        Properties = {
        //            {propertieName, jsonSchema.ToJsonSchemaProperty()} ,
        //        }
        //    };

        //    definitions.Add(apiname.ToPascalCase() + "Response", pushMessageList);
        //    return new NJsonSchema.JsonSchema
        //    {
        //        AllowAdditionalProperties = false,
        //        Type = NJsonSchema.JsonObjectType.Object,
        //        Reference = pushMessageList,
        //        //Definitions = { { "PushMessageList", pushMessageList } }
        //    };
        //}

        /// <summary>
        /// https://open.1688.com/api/apidocdetail.htm?id=aliexpress.open:api.getChildrenPostCategoryById-1 
        /// </summary>
        /// <returns></returns>
        private NJsonSchema.JsonSchema getAliexpressOpenApiGetChildrenPostCategoryByIdV1(System.Collections.Generic.IDictionary<string, JsonSchema> definitions)
        {
            var jsonSchemaAeopPostCategoryList = new NJsonSchema.JsonSchema
            {
                AllowAdditionalProperties = false,
                Type = NJsonSchema.JsonObjectType.Object,
                Properties = {
                    { "names", DictionarySchema(typeof(string)) .ToJsonSchemaProperty()} ,
                    { "level", NJsonSchema.JsonSchema.FromType<int>().ToJsonSchemaProperty() } ,
                    { "id", NJsonSchema.JsonSchema.FromType<int>().ToJsonSchemaProperty()} ,
                    { "isleaf", NJsonSchema.JsonSchema.FromType<bool>().ToJsonSchemaProperty() }
                }
            };


            var jsonSchema = new NJsonSchema.JsonSchema
            {
                AllowAdditionalProperties = false,
                Type = NJsonSchema.JsonObjectType.Object,
                Properties = {
                    { "aeopPostCategoryList",makeArraySchemaType( jsonSchemaAeopPostCategoryList,1).ToJsonSchemaProperty()} ,
                    { "success", NJsonSchema.JsonSchema.FromType<bool>().ToJsonSchemaProperty() }
                }
            };

            definitions.Add("ApiGetChildrenPostCategoryByIdResult", jsonSchema);
            return new NJsonSchema.JsonSchema
            {
                AllowAdditionalProperties = false,
                Type = NJsonSchema.JsonObjectType.Object,
                Reference = jsonSchema
            };
        }
        private System.Collections.Generic.Dictionary<string, NJsonSchema.JsonSchema> keyValuePairstModelInfo = new System.Collections.Generic.Dictionary<string, NJsonSchema.JsonSchema>();
        private NJsonSchema.JsonSchema createJsonSchema(string @namespace, string apiname, int version, ModelInfoResult[] modelInfoResult)
        {
            var jsonSchema = new NJsonSchema.JsonSchema
            {
                AllowAdditionalProperties = false,
                Type = NJsonSchema.JsonObjectType.Object
            };
            createJsonSchema(jsonSchema, @namespace, apiname, version, modelInfoResult);
            return jsonSchema;
        }
        private void createJsonSchema(NJsonSchema.JsonSchema jsonSchema, string @namespace, string apiname, int version, ModelInfoResult[] modelInfoResult)
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
                        jsonSchema.Properties.Add(item.Name, getJsonSchemaProperty(jsonSchema.Definitions, @namespace, apiname, version, item.Name, item.Type, item.DefaultValue, false, item.Description?.过滤特殊字符()));
                    }
                }
            }
        }
        private NJsonSchema.JsonSchemaProperty getJsonSchemaProperty(System.Collections.Generic.IDictionary<string, JsonSchema> definitions, string @namespace, string apiname, int version,
            string name, string type, object defaultValue, bool required, string description)
        {
            var tt = getSchema(definitions, @namespace, apiname, version, name, type, description);
            var r = tt.ToJsonSchemaProperty();
            r.AllowAdditionalProperties = false;
            r.Description = description;
            r.IsRequired = required;
            //Default = defaultValue

            return r;
        }

        //private System.Collections.Generic.List<string> vs = new System.Collections.Generic.List<string>();
        private System.Collections.Concurrent.ConcurrentDictionary<string, NJsonSchema.JsonSchema> keyValuePairs = new System.Collections.Concurrent.ConcurrentDictionary<string, NJsonSchema.JsonSchema>();

        #region 类型转换
        private NJsonSchema.JsonSchema getMessageTypeToSchema(System.Collections.Generic.IDictionary<string, JsonSchema> definitions, string @namespace, string apiname, int version, string type, string description)
        {
            if (type?.StartsWith("message:") != true) return null;
            var typeName = type.Replace("message:", "");

            var _type = 获取数组维度(typeName);

            var key = @namespace + apiname + _type.type;
            if (keyValuePairstModelInfo.ContainsKey(key))
            {
                return makeArraySchemaType(new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = keyValuePairstModelInfo[key] }, _type.arrLength);
            }
            else
            {
                var modelInfo = AlibabaDataCache.GetModelInfoByCacheAsync(@namespace, apiname, version, _type.type).GetAwaiter().GetResult();
                var modelInfoResult = modelInfo.Result;

                var md5key = modelInfoResult == null ? null : string.Join("", modelInfoResult?.Select(f => f.Name + f.Type));
                if (!string.IsNullOrWhiteSpace(md5key) && keyValuePairstModelInfo.ContainsKey(md5key))
                {
                    return makeArraySchemaType(new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = keyValuePairstModelInfo[md5key] }, _type.arrLength);
                }
                else
                {
                    var jsonSchema = new NJsonSchema.JsonSchema
                    {
                        AllowAdditionalProperties = false,
                        Type = NJsonSchema.JsonObjectType.Object
                    };
                    keyValuePairstModelInfo.Add(key, jsonSchema);
                    if (!string.IsNullOrWhiteSpace(md5key))
                        keyValuePairstModelInfo.Add(md5key, jsonSchema);
                    createJsonSchema(jsonSchema, @namespace, apiname, version, modelInfoResult);
                    jsonSchema.Description = (string.IsNullOrWhiteSpace(description) ? "" : description + "\r\n") + $"{modelInfo.ErrMsg}\r\n namespace:{@namespace},apiname:{apiname},version:{version},typeName:{_type.type}";

                    if (definitions != null)
                    {
                        var _typeName = _type.type?.ToPascalCase();
                        if (!definitions.ContainsKey(_typeName))
                            definitions.Add(_typeName, jsonSchema);
                        else
                        {
                            definitions.Add((@namespace + "." + apiname)?.ToPascalCase() + _typeName, jsonSchema);
                            //var dfsdfsd = document.Definitions[_typeName];
                        }
                    }


                    return makeArraySchemaType(new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = jsonSchema }, _type.arrLength);
                }
            }
        }

        private NJsonSchema.JsonSchema getSystemTypeToSchema(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;

            var _type = 获取数组维度(type);
            switch (_type.type?.ToLower())
            {
                case "int":
                case "integer":
                case "java.lang.integer":
                    return makeArraySystemType(typeof(int), _type.arrLength);
                case "java.lang.float":
                    return makeArraySystemType(typeof(float), _type.arrLength);
                case "long":
                case "java.lang.long":
                    return makeArraySystemType(typeof(long), _type.arrLength);
                case "java.lang.double":
                case "double":
                    return makeArraySystemType(typeof(double), _type.arrLength);
                case "bigdecimal":
                case "java.math.bigdecimal":
                    return makeArraySystemType(typeof(decimal), _type.arrLength);
                case "boolean":
                case "java.lang.boolean":
                    return makeArraySystemType(typeof(bool), _type.arrLength);
                case "date":
                case "java.util.date":
                    return makeArraySystemType(typeof(DateTime), _type.arrLength);
                case "byte":
                case "java.lang.byte":
                case "b":
                    return makeArraySystemType(typeof(byte), _type.arrLength);
                case "string":
                case "java.lang.string":
                case "ljava.lang.string":
                    return makeArraySystemType(typeof(string), _type.arrLength);
                case "inputstream":
                case "java.io.inputstream":
                    return makeArraySystemType(typeof(byte[]), _type.arrLength);
                //return makeArraySchemaType(FileSchema(), _type.arrLength);
                case "t":
                case "object":
                case "java.lang.object":
                case "com.alibaba.fastjson.jsonobject":
                    return makeArraySystemType(typeof(object), _type.arrLength);
                case "java.util.list":
                case "list":
                case "java.util.set":
                    return makeArraySystemType(typeof(string[]), _type.arrLength);
                case "map":
                case "java.util.map":
                    return DictionarySchema(typeof(object));// makeArraySchemaType(DictionarySchema(typeof(object)), _type.arrLength);
                case "java.lang.throwable":
                case "java.lang.void":
                    return makeArraySystemType(typeof(object), _type.arrLength);


                default:
                    return makeArraySystemType(typeof(object), 0);
            }
        }
        private NJsonSchema.JsonSchema DictionarySchema(Type type)
        {
            //var jsonSchema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.Object, AllowAdditionalProperties = false };
            //jsonSchema.AdditionalPropertiesSchema = NJsonSchema.JsonSchema.FromType(type);
            var jsonSchema = NJsonSchema.JsonSchema.FromType<System.Collections.Generic.Dictionary<string, string>>();
            return jsonSchema;
        }
        //private NJsonSchema.JsonSchema FileSchema()
        //{
        //    //var jsonSchema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.String, Format = JsonFormatStrings.Binary };
        //    var jsonSchema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.File, AllowAdditionalProperties = false };
        //    return jsonSchema;
        //}
        //private NJsonSchema.JsonSchema DictionarySchema(NJsonSchema.JsonSchema jsonSchemaItem)
        //{
        //    var jsonSchema = new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.Object, AllowAdditionalProperties = false };
        //    jsonSchema.AdditionalPropertiesSchema = jsonSchemaItem;
        //    return jsonSchema;
        //}
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
        public string ToCSharpCode(OpenApiDocument document)
        {
            //System.Net.WebClient wclient = new System.Net.WebClient();
            //var document = await OpenApiDocument.FromJsonAsync(wclient.DownloadString("Https://SwaggerSpecificationURL.json"));
            //wclient.Dispose();

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = "AlibabaApiClient",
                CSharpGeneratorSettings =
                {
                    Namespace = "AlibabaSDK"
                },
                GenerateSyncMethods = true
            };
            //settings.CodeGeneratorSettings.TemplateDirectory = "";
            settings.CodeGeneratorSettings.GenerateDefaultValues = true;
            settings.CodeGeneratorSettings.PropertyNameGenerator = new MyCSharpPropertyNameGenerator();

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            return code;
        }
        public string ToCSharpCodeClient(OpenApiDocument document, string _namespace, string className, string[] namespaceUsages)
        {
            //System.Net.WebClient wclient = new System.Net.WebClient();
            //var document = await OpenApiDocument.FromJsonAsync(wclient.DownloadString("Https://SwaggerSpecificationURL.json"));
            //wclient.Dispose();

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = className,//"AlibabaApiClient",
                CSharpGeneratorSettings =
                {
                    Namespace = _namespace//"AlibabaSDK"
                },
                GenerateSyncMethods = true,
                GenerateDtoTypes = false,
                GenerateClientInterfaces = true,
                GenerateExceptionClasses = false,
                GenerateOptionalParameters = true,
                AdditionalNamespaceUsages = namespaceUsages//new[] { "AlibabaSDK.Models" }
            };
            //settings.CodeGeneratorSettings.TemplateDirectory = "";
            settings.CodeGeneratorSettings.GenerateDefaultValues = true;
            settings.CodeGeneratorSettings.PropertyNameGenerator = new MyCSharpPropertyNameGenerator();
            settings.CodeGeneratorSettings.TemplateDirectory = @"D:\Users\mccj\Source\Repos\AlibabaSDKFor1688\src\tools\AlibabaSDKFor1688GenerationApp\Templates";

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            return code;
        }
        public string ToCSharpCodeModels(OpenApiDocument document, string _namespace)
        {
            //System.Net.WebClient wclient = new System.Net.WebClient();
            //var document = await OpenApiDocument.FromJsonAsync(wclient.DownloadString("Https://SwaggerSpecificationURL.json"));
            //wclient.Dispose();

            var settings = new CSharpClientGeneratorSettings
            {
                CSharpGeneratorSettings =
                {
                    Namespace = _namespace//"AlibabaSDK.Models"
                },
                GenerateClientClasses = false
            };
            //settings.CodeGeneratorSettings.TemplateDirectory = "";
            settings.CodeGeneratorSettings.GenerateDefaultValues = true;
            settings.CodeGeneratorSettings.PropertyNameGenerator = new MyCSharpPropertyNameGenerator();

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

    public class eeeeeeeee
    {
        public Datum ApiInfo { get; set; }
        public Base2Response<DetailResult> ApiDetail { get; set; }
    }
    public class MyCSharpPropertyNameGenerator : NJsonSchema.CodeGeneration.CSharp.CSharpPropertyNameGenerator
    {
        public override string Generate(JsonSchemaProperty property)
        {
            var name = base.Generate(property);
            return name.ToPascalCase();
        }
    }
}
