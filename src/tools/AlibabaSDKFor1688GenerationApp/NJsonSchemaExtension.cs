using Newtonsoft.Json;
using NJsonSchema;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class NJsonSchemaExtension
    {
        public static JsonSchemaProperty ToJsonSchemaProperty(this JsonSchema jsonSchema,System.Action<JsonSchemaProperty> action=null)
        {
            var jsonSchemaProperty = new JsonSchemaProperty
            {
                Type = jsonSchema.Type,
                Format = jsonSchema.Format,
                MinLength = jsonSchema.MinLength,
                Title = jsonSchema.Title,
                Reference = jsonSchema.Reference,
                Item = jsonSchema.Item,
            };
            foreach (var item in jsonSchema.Properties)
            {
                jsonSchemaProperty.Properties.Add(item);
            }
            action?.Invoke(jsonSchemaProperty);
            return jsonSchemaProperty;
        }
    }
}