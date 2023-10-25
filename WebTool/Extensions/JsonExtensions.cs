using System;
using System.Text.Json;

namespace WebTool
{
    internal static class JsonExtensions
    {
        private static void EnsurePropertyName(this Utf8JsonReader reader, string propertyName)
        {
            string jsonString = reader.GetString();

            if (jsonString?.IgEquals(propertyName) != true)
            {
                throw new JsonException("Invalid format");
            }
        }

        public static void EnsureTokenType(this Utf8JsonReader reader, JsonTokenType tokenType)
        {
            if (reader.TokenType != tokenType)
            {
                throw new JsonException("Invalid format");
            }
        }

        private static string GetJsonPropertyName(this string propertyName, JsonSerializerOptions options)
        {
            return options?.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
        }

        public static object GetValue(this Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            object value = reader.TokenType != JsonTokenType.Null
                ? JsonSerializer.Deserialize(ref reader, type, options)
                : default;

            return value;
        }

        private static TValue GetValue<TValue>(this Utf8JsonReader reader, JsonSerializerOptions options)
        {
            object value = reader.GetValue(typeof(TValue), options);

            return value != null ? (TValue)value : default;
        }

        public static void ReadPropertyName(this ref Utf8JsonReader reader, string propertyName)
        {
            reader.EnsureTokenType(JsonTokenType.PropertyName);
            reader.EnsurePropertyName(propertyName);
            reader.Read();
        }

        public static TValue ReadPropertyValue<TValue>(this ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            TValue value = reader.GetValue<TValue>(options);
            reader.Read();

            return value;
        }

        public static void WritePropertyName(this Utf8JsonWriter writer, string propertyName, JsonSerializerOptions options)
        {
            string name = propertyName.GetJsonPropertyName(options);

            writer.WritePropertyName(name);
        }
    }
}