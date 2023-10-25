using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebTool
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(HtmlCard), typeDiscriminator: "Card")]
    [JsonDerivedType(typeof(HtmlCardsContainer), typeDiscriminator: "Cards")]
    [JsonDerivedType(typeof(HtmlSection), typeDiscriminator: "Section")]
    [JsonDerivedType(typeof(HtmlSeoSection), typeDiscriminator: "SeoSection")]
    [JsonDerivedType(typeof(HtmlTab), typeDiscriminator: "Tab")]
    [JsonDerivedType(typeof(HtmlTable), typeDiscriminator: "Table")]
    [JsonDerivedType(typeof(HtmlTableCell), typeDiscriminator: "TableCell")]
    [JsonDerivedType(typeof(HtmlTableRow), typeDiscriminator: "TableRow")]
    [JsonDerivedType(typeof(HtmlTabsContainer), typeDiscriminator: "Tabs")]
    public abstract class HtmlContainer
    {
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Deleted { get; set; }

        [JsonPropertyOrder(3)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HtmlElementCollection Elements { get; set; }

        [JsonPropertyOrder(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string Id { get; set; }

        [JsonPropertyOrder(2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Title { get; set; }

        public HtmlContainer()
        {
            Id = Guid.NewGuid().ToString();
        }

        public HtmlContainer(HtmlContainer container, bool clean)
        {
            Id = Guid.NewGuid().ToString();
            Elements = new HtmlElementCollection(container.Elements.Select(e => new HtmlElement(e, clean)).ToList());

            if (!clean)
            {
                Deleted = container.Deleted;
                Title = container.Title;
            }
        }

        public string GetDiscriminator()
        {
            switch (this)
            {
                case HtmlCard:
                    return "Card";
                case HtmlCardsContainer:
                    return "Cards";
                case HtmlSection:
                    return "Section";
                case HtmlSeoSection:
                    return "SeoSection";
                case HtmlTab:
                    return "Tab";
                case HtmlTable:
                    return "Table";
                case HtmlTableCell:
                    return "TableCell";
                case HtmlTableRow:
                    return "TableRow";
                case HtmlTabsContainer:
                    return "Tabs";
            }

            return null;
        }
    }
}
