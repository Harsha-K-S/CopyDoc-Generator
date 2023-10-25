using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebTool
{
    public class HtmlTableRow : HtmlContainer
    {
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HtmlTableCell> Cells { get; set; }

        public HtmlTableRow(List<HtmlTableCell> cells)
        {
            Cells = cells;
        }
    }
}
