using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebTool
{
    public class ApplicationUserJsonConverter : JsonConverter<ApplicationUser>
    {
        public override ApplicationUser Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ApplicationUser user = new ApplicationUser();

            reader.EnsureTokenType(JsonTokenType.StartObject);
            reader.Read();

            // Id
            reader.ReadPropertyName(nameof(user.Id));
            user.Id = reader.ReadPropertyValue<string>(options);

            // Name
            reader.ReadPropertyName(nameof(user.Name));
            user.Name = reader.ReadPropertyValue<string>(options);

            // UserName
            reader.ReadPropertyName(nameof(user.UserName));
            user.UserName = reader.ReadPropertyValue<string>(options);

            // Email
            reader.ReadPropertyName(nameof(user.Email));
            user.Email = reader.ReadPropertyValue<string>(options);

            // Password
            reader.ReadPropertyName(nameof(user.Password));
            user.Password = reader.ReadPropertyValue<string>(options);

            // Role
            reader.ReadPropertyName(nameof(user.Role));
            user.Role = reader.ReadPropertyValue<string>(options);

            reader.EnsureTokenType(JsonTokenType.EndObject);

            return user;
        }

        public override void Write(Utf8JsonWriter writer, ApplicationUser user, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Id
            writer.WritePropertyName(nameof(user.Id), options);
            writer.WriteStringValue(user.Id);

            // Name
            writer.WritePropertyName(nameof(user.Name), options);
            writer.WriteStringValue(user.Name);

            // UserName
            writer.WritePropertyName(nameof(user.UserName), options);
            writer.WriteStringValue(user.UserName);

            // Email
            writer.WritePropertyName(nameof(user.Email), options);
            writer.WriteStringValue(user.Email);

            // Password
            writer.WritePropertyName(nameof(user.Password), options);
            writer.WriteStringValue(user.Password);

            // Role
            writer.WritePropertyName(nameof(user.Role), options);
            writer.WriteStringValue(user.Role);

            writer.WriteEndObject();
        }
    }
}