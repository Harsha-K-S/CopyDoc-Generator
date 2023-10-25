using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebTool
{
    public class HtmlCardsContainer : HtmlContainer
    {
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HtmlCard> Cards { get; set; }

        public HtmlCardsContainer(List<HtmlCard> cards)
        {
            Cards = cards.ToList();
        }

        public bool RemoveCardById(string cardId)
        {
            HtmlCard card = Cards.SingleOrDefault(c => c.Id == cardId);
            if (card != null)
            {
                card.Deleted = true;
            }

            return card != null;
        }
    }
}
