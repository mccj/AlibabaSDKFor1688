﻿using ConsoleApp2.sss;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiDocument = new ApiDocument();



            //var rrr = new OpenApiDocument();
            //rrr.Definitions["tt"] = new NJsonSchema.JsonSchema() { AllowAdditionalProperties = false };

            //rrr.Definitions["tt"].Properties["aa0"] = NJsonSchema.JsonSchema.FromType<int[,,,,,]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa1"] = NJsonSchema.JsonSchema.FromType<int[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa2"] = NJsonSchema.JsonSchema.FromType<float[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa3"] = NJsonSchema.JsonSchema.FromType<long[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa4"] = NJsonSchema.JsonSchema.FromType<double[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa5"] = NJsonSchema.JsonSchema.FromType<decimal[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa6"] = NJsonSchema.JsonSchema.FromType<bool[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa7"] = NJsonSchema.JsonSchema.FromType<DateTime[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa8"] = NJsonSchema.JsonSchema.FromType<byte[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa9"] = NJsonSchema.JsonSchema.FromType<string[][][]>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["aa10"] = NJsonSchema.JsonSchema.FromType<object[][][]>().ToJsonSchemaProperty();

            //rrr.Definitions["ee1"] = new NJsonSchema.JsonSchema { };
            //rrr.Definitions["ee2"] = new NJsonSchema.JsonSchema { };
            //rrr.Definitions["ee1"].Definitions.Add("aaa", new NJsonSchema.JsonSchema());
            //rrr.Definitions["ee2"].Definitions.Add("aaa", new NJsonSchema.JsonSchema());

            //rrr.Definitions["tt"].Properties["a3"] = NJsonSchema.JsonSchema.FromType<System.Collections.Generic.Dictionary<string, string>>().ToJsonSchemaProperty();
            //rrr.Definitions["tt"].Properties["a4"] = new NJsonSchema.JsonSchemaProperty { Type = NJsonSchema.JsonObjectType.Object };
            //rrr.Definitions["tt"].Properties["a4"].AdditionalPropertiesSchema = NJsonSchema.JsonSchema.FromType<decimal>();

            //var dddddd = rrr.ToJson();
            //var sdfdf = apiDocument.ToCSharpCode(rrr);


            var document = apiDocument.创建文档();

            Console.WriteLine("转换成 json 文件并保存");
            var json = document.ToJson();
            System.IO.File.WriteAllText(@"..\..\..\..\..\sdk\AlibabaSDKFor1688\AlibabaClient.json", json);

            Console.WriteLine("转换成 Client 代码并保存");
            var csharpClientCode = apiDocument.ToCSharpCodeClient(document);
            System.IO.File.WriteAllText(@"..\..\..\..\..\sdk\AlibabaSDKFor1688\AlibabaClientGeneration.cs", csharpClientCode);

            Console.WriteLine("转换成 ClientModels 代码并保存");
            var csharpModelsCode = apiDocument.ToCSharpCodeModels(document);
            System.IO.File.WriteAllText(@"..\..\..\..\..\sdk\AlibabaSDKFor1688\AlibabaClientModelsGeneration.cs", csharpModelsCode);
            //var code2 = apiDocument.ToTypeScriptCode(document);

            Console.WriteLine("Hello World!");
        }
        public class aaaa
        {

        }
    }
}
