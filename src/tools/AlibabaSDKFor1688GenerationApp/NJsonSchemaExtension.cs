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
                AllowAdditionalProperties = jsonSchema.AllowAdditionalProperties,
                AdditionalPropertiesSchema = jsonSchema.AdditionalPropertiesSchema
            };
            foreach (var item in jsonSchema.Properties)
            {
                jsonSchemaProperty.Properties.Add(item);
            }

            //var json = jsonSchema.ToJson();
            //var jsonSchemaProperty = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonSchemaProperty>(json);
            action?.Invoke(jsonSchemaProperty);
            return jsonSchemaProperty;
        }
    }
}