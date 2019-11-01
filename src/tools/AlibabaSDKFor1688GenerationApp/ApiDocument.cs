using ConsoleApp2.sss;
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
        private readonly AlibabaApi alibabaApi = new AlibabaApi();
        public OpenApiDocument 创建文档()
        {
            //var document1 = OpenApiDocument.FromFileAsync(@"Data\swagger.json").Result;
            var document = 创建().GetAwaiter().GetResult();

            keyValuePairs____.Sort();
            var ddd = string.Join("\r\n", keyValuePairs____);

            return document;
        }
        private async Task<OpenApiDocument> 创建()
        {
            var document = new OpenApiDocument();

            document.Servers.Add(new OpenApiServer { Url = "https://gw.open.1688.com/" });

            var publicApisAll = await alibabaApi.ListPublicApiAsync();
            var publicApis = publicApisAll.Data.GroupBy(f => new { f.Namespace, f.Name }).Select(f => new Datum { Name = f.Key.Name, Namespace = f.Key.Namespace, Version = f.Max(ff => ff.Version) }).ToArray();
            //var ss2 = alibabaApi.GetAllApiCategory().Result.Result;
            //var ss3 = ss2.Select(f => GetAopApiListByCategory(f.AopCategoryCode).Result.Result).ToArray();
            var publicApiDetails = publicApis.Select(f => new { ApiInfo = f, ApiDetail = GetApiDetail(f.Namespace, f.Name, f.Version) })/*.Skip(587).Take(1)*/.ToArray();

            for (int i = 0; i < publicApiDetails.Length; i++)
            {
                var publicApi = publicApiDetails[i];
                Console.WriteLine((i + 1) + "." + publicApi.ApiInfo.Name);

                var apiDetailResponse = await publicApi.ApiDetail;
                var apiDetail = apiDetailResponse.Result;
                //var apiArguments = await GetApiArguments(item.Namespace, item.Name, item.Version);

                var description = 过滤特殊字符($" \r\n{apiDetail.DisplayName}\r\n{apiDetail.Description}\r\n\r\n文档: https://open.1688.com/api/apidocdetail.htm?id={apiDetail.OceanApiId} \r\n调试:https://open.1688.com/api/apiTool.htm?ns={apiDetail.Namespace}&n={apiDetail.Name}&v={apiDetail.Version} \r\n ");
                var openApiOperation = new OpenApiOperation
                {
                    Summary = description,
                    //Description = item.Description,
                    OperationId = 转换驼峰命名方式(apiDetail.Name)
                };

                //if (openApiOperation.OperationId == "AlibabaTradeGetLogisticsInfosBuyerView") { }
                //else
                {
                    //var sss = new OpenApiSecurityRequirement();
                    //sss.Add("zzzzzzzzzzzzzz", new[] { "fdfff" });
                    //openApiOperation.Security.Add(sss);

                    openApiOperation.Tags.Add("namespace=" + apiDetail.Namespace);
                    openApiOperation.Tags.Add("name=" + apiDetail.Name);
                    openApiOperation.Tags.Add("version=" + apiDetail.Version);
                    openApiOperation.Tags.Add("oceanApiId=" + apiDetail.OceanApiId);
                    openApiOperation.Tags.Add("billFlag=" + apiDetail.BillFlag);
                    openApiOperation.Tags.Add("neddAuth=" + apiDetail.NeddAuth);

                    //foreach (var item in apiDetail.ApiSystemParamVOList)
                    //{
                    //}
                    foreach (var item in apiDetail.ApiAppParamVOList)
                    {
                        openApiOperation.Parameters.Add(new OpenApiParameter
                        {
                            Kind = OpenApiParameterKind.FormData,
                            Name = item.Name,
                            IsRequired = item.Required == true,
                            Default = item.DefaultValue,
                            Description = 过滤特殊字符(item.Description),
                            Schema = getSchema(document, apiDetail.Namespace, apiDetail.Name, apiDetail.Version, item.Type, 过滤特殊字符(item.Description))
                        });
                    }
                    if (apiDetail.ApiReturnParamVOList.Length <= 1)
                    {
                        var item = apiDetail.ApiReturnParamVOList.SingleOrDefault();
                        var openApiResponse = new OpenApiResponse
                        {
                            Description = 过滤特殊字符(item?.Description)
                        };
                        if (item != null)
                            openApiResponse.Content.Add("application/json", new OpenApiMediaType { Schema = getSchema(document, apiDetail.Namespace, apiDetail.Name, apiDetail.Version, item.Type, item.Description) });
                        openApiOperation.Responses.Add("200", openApiResponse);
                    }
                    else
                    {
                        var jsonSchema = createJsonSchema(document, apiDetail.Namespace, apiDetail.Name, apiDetail.Version, apiDetail.ApiReturnParamVOList);
                        jsonSchema.Description = description;

                        var typeName = openApiOperation.OperationId + "Result";
                        if (!document.Definitions.ContainsKey(typeName))
                            document.Definitions.Add(typeName, jsonSchema);
                        else
                        {
                            document.Definitions.Add(转换驼峰命名方式(apiDetail.Namespace) + typeName, jsonSchema);

                            //var dfsdfsd = document.Definitions[jjjjj];
                        }
                        var openApiResponse = new OpenApiResponse
                        {
                            //Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                        };
                        openApiResponse.Content.Add("application/json", new OpenApiMediaType { Schema = new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = jsonSchema } });
                        openApiOperation.Responses.Add("200", openApiResponse);
                    }
                    if (apiDetail.ApiErrorCodeVOList.Any())
                    {
                        //var dfsfsd = NJsonSchema.JsonSchema.FromType<rrr>();
                        //var jsonSchema = new NJsonSchema.JsonSchema
                        //{
                        //    AllowAdditionalProperties = false,
                        //    Type = NJsonSchema.JsonObjectType.String,
                        //    Description = description
                        //};
                        //foreach (var item2 in item.ApiErrorCodeVOList)
                        //{
                        //    jsonSchema.Enumeration.Add(int.Parse(item2.Code));
                        //    jsonSchema.EnumerationNames.Add(item2.Desc + "_" + item2.Code);
                        //}
                        //var jjjjj = openApiOperation.OperationId + "ErrorCode";
                        //if (!document.Components.Schemas.ContainsKey(jjjjj))
                        //    document.Components.Schemas.Add(jjjjj, jsonSchema);
                        //else
                        //{
                        //    document.Components.Schemas.Add(转换驼峰命名方式(item.Namespace) + jjjjj, jsonSchema);
                        //    //var dfsdfsd = document.Definitions[jjjjj];
                        //}
                    }

                    foreach (var item2 in apiDetail.ApiDocSampleVOList)
                    {

                    }

                    document.Paths.Add($"/openapi/param2/{apiDetail.Version}/{apiDetail.Namespace}/{apiDetail.Name}/{{AppKey}}", new OpenApiPathItem
                    {
                        { "post",openApiOperation}
                    });
                }
            }

            return document;
        }

        private string 转换驼峰命名方式(string name)
        {
            var charlist = name.ToArray();
            for (int i = 0; i < charlist.Length; i++)
            {
                if (i == 0) { charlist[i] = char.ToUpper(charlist[i]); }
                else if (charlist[i] == '.' && i <= charlist.Length - 1)
                {
                    charlist[i + 1] = char.ToUpper(charlist[i + 1]);
                }
            }
            return new string(charlist).Replace(".", "");
        }
        private System.Collections.Generic.List<string> keyValuePairs____ = new System.Collections.Generic.List<string>();
        private NJsonSchema.JsonSchema getSchema(OpenApiDocument document, string @namespace, string apiname, int version, string type, string description)
        {
            var jsonSchema = keyValuePairs.GetOrAdd(@namespace + apiname + type, t =>
             {
                 return getMessageSchema(document, @namespace, apiname, version, type) ?? toSchema(type);
             });

            if (jsonSchema == null)
            {
                keyValuePairs____.Add(type + "\t\t\t=\t\t\t" + @namespace + ":" + apiname + "-" + version);
                //TODO:eeeeeeee
                return NJsonSchema.JsonSchema.FromType<object>();
            }
            return jsonSchema;
        }
        private NJsonSchema.JsonSchema toSchema(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                //TODO:eeeeeeee
                return NJsonSchema.JsonSchema.FromType<object>();
            }
            var a = 0;
            for (int i = 0; i < 100; i++)
            {
                if (type.EndsWith("[]"))
                {
                    a++;
                    type = type.Remove(type.Length - 2);
                }
                else
                {
                    break;
                }
            }

            //if (type.StartsWith("message:"))
            //{
            //    return keyValuePairs.GetOrAdd(type, t =>
            //    {
            //        return getMessageSchema(t);
            //    });
            //}
            //else
            {
                var typp = getType(type);
                if (typp != null)
                {
                    for (int i = 0; i < a; i++)
                    {
                        typp = typp.MakeArrayType();
                    }

                    return NJsonSchema.JsonSchema.FromType(typp);
                }
            }

            //TODO:eeeeeeee
            return null;
        }
        private System.Collections.Generic.Dictionary<string, NJsonSchema.JsonSchema> keyValuePairstModelInfo = new System.Collections.Generic.Dictionary<string, NJsonSchema.JsonSchema>();
        private NJsonSchema.JsonSchema getMessageSchema(OpenApiDocument document, string @namespace, string apiname, int version, string type)
        {
            if (type?.StartsWith("message:") != true) return null;
            var typeName = type.Replace("message:", "");

            var a = 0;
            for (int i = 0; i < 100; i++)
            {
                if (typeName.EndsWith("[]"))
                {
                    a++;
                    typeName = typeName.Remove(typeName.Length - 2);
                }
                else
                {
                    break;
                }
            }

            if (a > 0)
            {
                //new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Type = NJsonSchema.JsonObjectType.Array }.Item.Reference;
                //TODO:eeeeeeee
            }

            var key = @namespace + apiname + typeName;
            if (keyValuePairstModelInfo.ContainsKey(key))
            {
                return new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = keyValuePairstModelInfo[key] };
            }
            else
            {
                var modelInfo = GetModelInfo(@namespace, apiname, version, typeName).GetAwaiter().GetResult();
                var modelInfoResult = modelInfo.Result;

                var jsonSchema = new NJsonSchema.JsonSchema
                {
                    AllowAdditionalProperties = false,
                    Type = NJsonSchema.JsonObjectType.Object
                };
                keyValuePairstModelInfo.Add(key, jsonSchema);
                createJsonSchema(jsonSchema, document, @namespace, apiname, version, modelInfoResult);
                jsonSchema.Description = $"{modelInfo.ErrMsg}\r\n namespace:{@namespace},apiname:{apiname},version:{version},typeName:{typeName}";

                var _typeName = 转换驼峰命名方式(typeName);
                if (!document.Definitions.ContainsKey(_typeName))
                    document.Definitions.Add(_typeName, jsonSchema);
                else
                {
                    document.Definitions.Add(转换驼峰命名方式(@namespace + "." + apiname) + _typeName, jsonSchema);
                    //var dfsdfsd = document.Definitions[_typeName];
                }
                return new NJsonSchema.JsonSchema { AllowAdditionalProperties = false, Reference = jsonSchema };
            }
        }
        private NJsonSchema.JsonSchema createJsonSchema(OpenApiDocument document, string @namespace, string apiname, int version, ModelInfoResult[] modelInfoResult)
        {
            var jsonSchema = new NJsonSchema.JsonSchema
            {
                AllowAdditionalProperties = false,
                Type = NJsonSchema.JsonObjectType.Object
            };
            createJsonSchema(jsonSchema, document, @namespace, apiname, version, modelInfoResult);
            return jsonSchema;
        }
        private void createJsonSchema(NJsonSchema.JsonSchema jsonSchema, OpenApiDocument document, string @namespace, string apiname, int version, ModelInfoResult[] modelInfoResult)
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
                        jsonSchema.Properties.Add(item.Name, getJsonSchemaProperty(document, @namespace, apiname, version, item.Name, item.Type, item.DefaultValue, false, 过滤特殊字符(item.Description)));
                    }
                }
            }
        }
        private NJsonSchema.JsonSchemaProperty getJsonSchemaProperty(OpenApiDocument document, string @namespace, string apiname, int version,
            string name, string type, object defaultValue, bool required, string description)
        {
            var r = new NJsonSchema.JsonSchemaProperty
            {
                //Name = name,
                AllowAdditionalProperties = false,
                Description = description,
                IsRequired = required,
                //Default = defaultValue
            };
            var tt = getSchema(document, @namespace, apiname, version, type, "");
            if (type.StartsWith("message:"))
            {
                r.Reference = tt.Reference;
            }
            else
            {
                //var tt = toSchema(type);
                if (tt != null)
                {
                    r.Type = tt.Type;
                    r.Format = tt.Format;
                    r.MinLength = tt.MinLength;
                    r.Title = tt.Title;
                }
            }
            return r;
        }

        //private System.Collections.Generic.List<string> vs = new System.Collections.Generic.List<string>();
        private System.Collections.Concurrent.ConcurrentDictionary<string, NJsonSchema.JsonSchema> keyValuePairs = new System.Collections.Concurrent.ConcurrentDictionary<string, NJsonSchema.JsonSchema>();
        private Type getType(string _type)
        {
            if (string.IsNullOrWhiteSpace(_type)) return null;

            switch (_type)
            {
                case "String":
                case "java.lang.String":
                    return typeof(string);
                case "[Ljava.lang.String;":
                    return typeof(string[]);
                case "Long":
                case "long":
                case "java.lang.Long":
                    return typeof(long);
                case "int":
                case "Integer":
                case "java.lang.Integer":
                    return typeof(int);
                case "java.lang.Float":
                    return typeof(float);
                case "Boolean":
                case "boolean":
                case "java.lang.Boolean":
                    return typeof(bool);
                case "java.lang.Double":
                case "Double":
                case "double":
                    return typeof(double);
                case "BigDecimal":
                case "java.math.BigDecimal":
                    return typeof(decimal);
                case "Date":
                case "java.util.Date":
                    return typeof(DateTime);
                case "byte":
                case "java.lang.Byte":
                    return typeof(byte);
                case "[B":
                case "InputStream":
                case "java.io.InputStream":
                    return typeof(byte[]);
                case "object":
                case "Object":
                case "java.lang.Object":
                case "com.alibaba.fastjson.JSONObject":
                    return typeof(object);
                case "java.util.List":
                case "List":
                    return typeof(System.Collections.Generic.List<string>);
                case "Map":
                case "java.util.Map":
                    return typeof(System.Collections.Generic.Dictionary<string, object>);
                case "java.util.Set":
                    return typeof(System.Collections.Generic.HashSet<string>);
                case "java.lang.Throwable":
                    return typeof(System.Exception);
                //case "java.io.InputStream":
                //case "InputStream":
                //case "T":
                //case "java.util.Set":


                default:
                    break;
            }
            //var v = $"case \"{ _type}\":";
            //if (!vs.Contains(v))
            //    vs.Add(v);

            //TODO:eeeeeeee
            return null;
        }

        private string 过滤特殊字符(string str)
        {
            return str?.Replace("\b", "").Replace("\u001b", "");
        }
        private async Task<BaseResponse<ArgumentData>> GetApiArguments(string @namespace, string name, int version)
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
        private async Task<Base2Response<ListByCategoryResult[]>> GetAopApiListByCategory(string aopApiCategory)
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
        private async Task<Base2Response<DetailResult>> GetApiDetail(string @namespace, string name, int version)
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
        private async Task<Base2Response<ModelInfoResult[]>> GetModelInfo(string @namespace, string apiname, int version, string typeName)
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
        public string ToCSharpCode(OpenApiDocument document)
        {
            //System.Net.WebClient wclient = new System.Net.WebClient();
            //var document = await OpenApiDocument.FromJsonAsync(wclient.DownloadString("Https://SwaggerSpecificationURL.json"));
            //wclient.Dispose();

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = "AlibabaClient",
                CSharpGeneratorSettings =
                {
                    Namespace = "AlibabaSDK"
                },
                GenerateSyncMethods = true
            };

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
    }
}
