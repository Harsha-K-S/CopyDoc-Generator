using System.Collections.Generic;
using System.Linq;

namespace WebTool
{
    public class HtmlContainerCollection : List<HtmlContainer>
    {
        public HtmlContainer CloneContainer(string containerId, out HtmlContainer parent)
        {
            parent = null;

            foreach (HtmlContainer container in this)
            {
                switch (container)
                {
                    case HtmlCardsContainer cardsContainer:
                        HtmlCard card = cardsContainer.Cards.SingleOrDefault(c => c.Id == containerId);
                        if (card != null)
                        {
                            parent = cardsContainer;
                            HtmlCard clone = new HtmlCard(card, true);
                            cardsContainer.Cards.Add(clone);
                            clone.Title = $"Card{cardsContainer.Cards.Count}";
                            return clone;
                        }
                        break;
                    case HtmlTabsContainer tabsContainer:
                        HtmlTab tab = tabsContainer.Tabs.SingleOrDefault(t => t.Id == containerId);
                        if (tab != null)
                        {
                            parent = tabsContainer;
                            HtmlTab clone = new HtmlTab(tab, true);
                            tabsContainer.Tabs.Add(clone);
                            return clone;
                        }
                        break;
                }
            }

            return null;
        }

        public bool DeleteElement(string elementId)
        {
            bool deleted = this.Any(container =>
            {
                switch (container)
                {
                    case HtmlCardsContainer cardsContainer:
                        return cardsContainer.Cards.Any(c => c.Elements.RemoveById(elementId))
                            || cardsContainer.RemoveCardById(elementId);
                    case HtmlSection:
                    case HtmlSeoSection:
                        return container.Elements.RemoveById(elementId);
                    case HtmlTabsContainer tabsContainer:
                        return tabsContainer.Tabs.Any(t => t.Elements.RemoveById(elementId))
                            || tabsContainer.RemoveTabById(elementId);
                }

                return false;
            });

            foreach (HtmlContainer container in this)
            {
                switch (container)
                {
                    case HtmlCardsContainer cardsContainer when !cardsContainer.Cards.Any(c => c.Deleted != true):
                    case HtmlTabsContainer tabsContainer when !tabsContainer.Tabs.Any(t => t.Deleted != true):
                        container.Deleted = true;
                        break;
                }
            }

            return deleted;
        }

        public bool UpdateElement(HtmlElement element)
        {
            HtmlElement existing = FindElementById(element.Id);
            if (existing == null)
            {
                return false;
            }

            // Alt
            if (element.Alt != null)
            {
                existing.AltPrev = existing.Alt;
                existing.Alt = element.Alt;
            }

            // DataVideoEmbed
            if (element.DataVideoEmbed != null)
            {
                existing.DataVideoEmbedPrev = existing.DataVideoEmbed;
                existing.DataVideoEmbed = element.DataVideoEmbed;
            }

            // Href
            if (element.Href != null)
            {
                existing.HrefPrev = existing.Href;
                existing.Href = element.Href;
            }

            // Src
            if (element.Src != null)
            {
                existing.SrcPrev = existing.Src;
                existing.Src = element.Src;
            }

            // Text
            if (element.Text != null)
            {
                existing.TextPrev = existing.Text;
                existing.Text = element.Text;
            }

            return true;
        }

        public HtmlContainer FindContainerById(string containerId)
        {
            foreach (HtmlContainer container in this)
            {
                HtmlContainer found = null;
                switch (container)
                {
                    case HtmlCardsContainer cardsContainer:
                        found = cardsContainer.Cards.SingleOrDefault(c => c.Id == containerId);
                        break;
                    case HtmlTabsContainer tabsContainer:
                        found = tabsContainer.Tabs.SingleOrDefault(t => t.Id == containerId);
                        break;
                }

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private HtmlElement FindElementById(string elementId)
        {
            foreach (HtmlContainer container in this)
            {
                HtmlElement found = null;
                switch (container)
                {
                    case HtmlCardsContainer cardsContainer:
                        found = cardsContainer.Cards.SelectMany(c => c.Elements).SingleOrDefault(e => e.Id == elementId);
                        break;
                    case HtmlSection:
                    case HtmlSeoSection:
                        found = container.Elements.SingleOrDefault(e => e.Id == elementId);
                        break;
                    case HtmlTable table:
                        found = table.Rows.SelectMany(r => r.Cells.SelectMany(c => c.Elements)).SingleOrDefault(e => e.Id == elementId);
                        break;
                    case HtmlTableRow tableRow:
                        found = tableRow.Cells.SelectMany(c => c.Elements).SingleOrDefault(e => e.Id == elementId);
                        break;
                    case HtmlTableCell tableCell:
                        found = tableCell.Elements.SingleOrDefault(e => e.Id == elementId);
                        break;
                    case HtmlTabsContainer tabsContainer:
                        found = tabsContainer.Tabs.SelectMany(t => t.Elements).SingleOrDefault(e => e.Id == elementId);
                        break;
                }

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }
    }
}
