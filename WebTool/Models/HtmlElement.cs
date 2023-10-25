using System;
using System.Text.Json.Serialization;

namespace WebTool
{
    public class HtmlElement
    {
        [JsonPropertyOrder(9)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Alt { get; set; }

        [JsonPropertyOrder(10)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AltPrev { get; set; }

        [JsonPropertyOrder(5)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string DataVideoEmbed { get; set; }

        [JsonPropertyOrder(6)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string DataVideoEmbedPrev { get; set; }

        [JsonPropertyOrder(13)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Deleted { get; set; }

        [JsonPropertyOrder(11)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Href { get; set; }

        [JsonPropertyOrder(12)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string HrefPrev { get; set; }

        [JsonPropertyOrder(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string Id { get; set; }

        [JsonPropertyOrder(2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string Name { get; set; }

        [JsonPropertyOrder(3)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Src { get; set; }

        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SrcPrev { get; set; }

        [JsonPropertyOrder(7)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Text { get; set; }

        [JsonPropertyOrder(8)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TextPrev { get; set; }

        public HtmlElement() { }

        public HtmlElement(HtmlElement element, bool clean)
        {
            Id = Guid.NewGuid().ToString();
            Name = element.Name;

            if (!clean)
            {
                Alt = element.Alt;
                AltPrev = element.AltPrev;
                DataVideoEmbed = element.DataVideoEmbed;
                DataVideoEmbedPrev = element.DataVideoEmbedPrev;
                Deleted = element.Deleted;
                Href = element.Href;
                HrefPrev = element.HrefPrev;
                Src = element.Src;
                SrcPrev = element.SrcPrev;
                Text = element.Text;
                TextPrev = element.TextPrev;
            }
        }
    }
}
