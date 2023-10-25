using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebTool
{
    [JsonConverter(typeof(HtmlElementCollectionJsonConverter))]
    public class HtmlElementCollection : List<HtmlElement>
    {
        public HtmlElementCollection(IEnumerable<HtmlElement> elements)
            : base(elements) { }

        public bool RemoveById(string elementId)
        {
            HtmlElement element = this.SingleOrDefault(e => e.Id == elementId);
            if (element != null)
            {
                element.Deleted = true;
            }

            return element != null;
        }
    }
}
