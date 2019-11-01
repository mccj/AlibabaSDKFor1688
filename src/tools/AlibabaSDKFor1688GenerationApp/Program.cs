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
    class Program
    {
        static void Main(string[] args)
        {
            var apiDocument = new ApiDocument();

            var rrr = new OpenApiDocument();
            rrr.Definitions["tt"] = new NJsonSchema.JsonSchema() { AllowAdditionalProperties =false};

            rrr.Definitions["tt"].Properties["a1"] = NJsonSchema.JsonSchema.FromType<string[]>().ToJsonSchemaProperty();
            rrr.Definitions["tt"].Properties["a3"] = NJsonSchema.JsonSchema.FromType<System.Collections.Generic.Dictionary<string, string>>().ToJsonSchemaProperty();
            rrr.Definitions["tt"].Properties["a4"] = new NJsonSchema.JsonSchemaProperty { Type = NJsonSchema.JsonObjectType.Object };
            rrr.Definitions["tt"].Properties["a4"].AdditionalPropertiesSchema=new NJsonSchema.JsonSchema { Type = NJsonSchema.JsonObjectType.String };


            var dddddd = rrr.ToJson();
            var sdfdf = apiDocument.ToCSharpCode(rrr);


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
