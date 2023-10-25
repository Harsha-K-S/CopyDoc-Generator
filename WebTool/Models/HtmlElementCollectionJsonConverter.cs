using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebTool
{
    public class HtmlElementCollectionJsonConverter : JsonConverter<HtmlElementCollection>
    {
        public override HtmlElementCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<HtmlElement> elements = JsonSerializer.Deserialize<List<HtmlElement>>(ref reader, options);

            return new HtmlElementCollection(elements);
        }

        public override void Write(Utf8JsonWriter writer, HtmlElementCollection elements, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, elements as List<HtmlElement>, options);
        }
    }
}
