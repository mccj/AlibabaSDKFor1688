using Newtonsoft.Json;
using NJsonSchema;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class NJsonSchemaExtension
    {
        public static JsonSchemaProperty ToJsonSchemaProperty(this JsonSchema jsonSchema)
        {
            return new JsonSchemaProperty
            {
                Type = jsonSchema.Type,
                Format = jsonSchema.Format,
                MinLength = jsonSchema.MinLength,
                Title = jsonSchema.Title,
                Reference = jsonSchema.Reference,
                Item = jsonSchema.Item
            };
        }
    }
}