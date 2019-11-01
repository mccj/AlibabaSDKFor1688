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

            var document = apiDocument.创建文档();

            var json = document.ToJson();
            System.IO.File.WriteAllText(@"..\..\..\..\..\sdk\AlibabaSDKFor1688\AlibabaClient.json", json);
            var code1 = apiDocument.ToCSharpCode(document);
            //var code2 = apiDocument.ToTypeScriptCode(document);
            System.IO.File.WriteAllText(@"..\..\..\..\..\sdk\AlibabaSDKFor1688\AlibabaClientGeneration.cs", code1);

            Console.WriteLine("Hello World!");
        }
    }
}
