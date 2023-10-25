using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebTool
{
    public class HtmlTable : HtmlContainer
    {
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HtmlTableRow> Rows { get; set; }

        public HtmlTable(List<HtmlTableRow> rows)
        {
            Rows = rows;
        }
    }
}
