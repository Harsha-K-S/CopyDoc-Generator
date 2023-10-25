using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebTool
{
    public class HtmlTabsContainer : HtmlContainer
    {
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HtmlTab> Tabs { get; set; }

        public HtmlTabsContainer(List<HtmlTab> tabs)
        {
            Tabs = tabs;
        }

        public bool RemoveTabById(string tabId)
        {
            HtmlTab tab = Tabs.SingleOrDefault(t => t.Id == tabId);
            if (tab != null)
            {
                tab.Deleted = true;
            }

            return tab != null;
        }
    }
}
