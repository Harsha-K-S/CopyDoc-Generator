using System.Text.Json.Serialization;

namespace WebTool
{
    public class HtmlTableCell : HtmlContainer
    {
        [JsonPropertyOrder(4)]
        public bool IsHeader { get; set; }
    }
}
