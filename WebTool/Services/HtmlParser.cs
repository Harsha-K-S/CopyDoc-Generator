using HtmlAgilityPack;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebTool
{
    public static class HtmlParser
    {
        public static HtmlContainerCollection Parse(string url)
        {
            HtmlDocument html = new HtmlDocument();

            using (IWebDriver driver = Selenium.CreateChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                driver.Navigate().GoToUrl(url);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                wait.Until(d => js.ExecuteScript("return document.readyState").Equals("complete"));
                string htmlText = (string)js.ExecuteScript("return document.documentElement.outerHTML;");
                html.LoadHtml(htmlText);
            }

            HtmlContainerCollection containers = new HtmlContainerCollection();

            // SEO details
            string pageTitle = html.DocumentNode.SelectSingleNode("//head/title")?.InnerHtml;
            string metaKeywords = html.DocumentNode.SelectSingleNode("//head/meta[@name='keywords']")?.GetAttributeValue("content", "");
            string metaDescription = html.DocumentNode.SelectSingleNode("//head/meta[@name='description']")?.GetAttributeValue("content", "");

            containers.Add(new HtmlSeoSection
            {
                Elements = new HtmlElementCollection(new List<HtmlElement>
                {
                    new HtmlElement
                    {
                        Id = Guid.NewGuid().ToString().NullIfUndefined(),
                        Name = "pagetitle",
                        Text = HttpUtility.HtmlDecode(pageTitle).Trim().NullIfUndefined()
                    },
                    new HtmlElement
                    {
                        Id = Guid.NewGuid().ToString().NullIfUndefined(),
                        Name = "metakeywords",
                        Text = HttpUtility.HtmlDecode(metaKeywords).Trim().NullIfUndefined()
                    },
                    new HtmlElement
                    {
                        Id = Guid.NewGuid().ToString().NullIfUndefined(),
                        Name = "metadescription",
                        Text = HttpUtility.HtmlDecode(metaDescription).Trim().NullIfUndefined()
                    }
                }),
                Title = "SEO Details"
            });

            List<HtmlNode> nodes = html.DocumentNode
                .SelectNodes("//div[contains(@class, 'mc-tabbed-cards-center-grid') or contains(@class, 'section') or contains(@class, 'row row-size1 row-bottom-size2')] | //section | //*[@role='tablist'] | //iframe | //table")
                .EmptyIfNull()
                .Where(n => n.InnerHtml.IsDefined())
                .ToList();

            foreach (HtmlNode node in nodes)
            {
                AdjustAttributes(node, url, "href");
                AdjustAttributes(node, url, "src");

                if (IsCardCollection(node) || IsCarousel(node))
                {
                    List<HtmlCard> cards = node.SelectNodes(".//*[contains(@class, 'card-container')]")
                        .Select((n, i) => new HtmlCard
                        {
                            Elements = CreateHtmlElementCollection(n.ChildNodes),
                            Title = $"Card{i + 1}"
                        })
                        .ToList();

                    containers.Add(new HtmlCardsContainer(cards)
                    {
                        Title = IsCarousel(node) ? GetFirstHeadlineTitle(node) : "Cards"
                    });

                    node.RemoveAllChildren();
                }
                else if (IsSection(node))
                {
                    HtmlElementCollection elements = CreateHtmlElementCollection(node.ChildNodes);
                    if (elements.Any())
                    {
                        containers.Add(new HtmlSection
                        {
                            Elements = elements,
                            Title = GetFirstHeadlineTitle(node) ?? $"Component{containers.Count + 1}"
                        });
                    }
                }
                else if (IsTabList(node))
                {
                    List<HtmlTab> tabs = new List<HtmlTab>();
                    HtmlNodeCollection buttons = node.SelectNodes(".//button[@role='tab']");
                    HtmlNodeCollection contents = node.Attributes["class"]?.Value.Contains("cards") == true
                        ? node.ParentNode.ParentNode.ParentNode.SelectNodes(".//div[contains(@class, 'mc-tabbed-cards-center__content') and not(contains(@class, 'mc-tabbed-cards-center__content-container'))]")
                        : node.ParentNode.ParentNode.SelectNodes(".//*[@role='tabpanel']");

                    if (buttons != null && contents != null)
                    {
                        for (int index = 0; index < buttons.Count; index++)
                        {
                            tabs.Add(new HtmlTab
                            {
                                Elements = CreateHtmlElementCollection(contents.ElementAt(index).ChildNodes),
                                Title = HttpUtility.HtmlDecode(buttons[index].InnerText).Trim().NullIfUndefined() ?? $"Tab{index + 1}"
                            });
                        }

                        buttons.ForEach(b => b.Remove());
                        contents.ForEach(c => c.Remove());
                        node.Remove();

                        containers.Add(new HtmlTabsContainer(tabs)
                        {
                            Title = "Tabs"
                        });
                    }
                }
                else if (IsTable(node))
                {
                    List<HtmlTableRow> rows = node.SelectNodes(".//tr")
                        .Select(n => new HtmlTableRow(n.SelectNodes(".//th | .//td")
                            .Select(n => new HtmlTableCell
                            {
                                Elements = new HtmlElementCollection(n.ChildNodes.Where(IsNotEmptyCell).Select(CreateHtmlElement)),
                                IsHeader = n.Name == "th"
                            })
                            .ToList()))
                        .ToList();

                    containers.Add(new HtmlTable(rows)
                    {
                        Title = "Table"
                    });
                }
            }

            return containers;
        }

        private static void AdjustAttributes(HtmlNode root, string baseUrl, string attrName)
        {
            List<HtmlAttribute> attributes =
                root.Descendants()
                    .Select(n => n.Attributes[attrName])
                    .Where(a => a != null)
                    .ToList();

            foreach (HtmlAttribute attribute in attributes)
            {
                attribute.Value = ConvertToAbsoluteUrl(baseUrl, attribute.Value);
            }
        }

        private static string ConvertToAbsoluteUrl(string baseUrl, string url)
        {
            Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);

            return uri.IsAbsoluteUri
                ? uri.ToString()
                : new Uri(new Uri(baseUrl), uri).ToString();
        }

        private static HtmlElementCollection CreateHtmlElementCollection(HtmlNodeCollection nodes)
        {
            return new HtmlElementCollection(
                nodes.Descendants()
                    .Where(n => IsNotEmpty(n) && !IsInsideTable(n))
                    .Select(CreateHtmlElement));
        }

        private static HtmlElement CreateHtmlElement(HtmlNode node)
        {
            return new HtmlElement
            {
                Alt = node.GetAttributeValue("alt", "").Trim().NullIfUndefined(),
                DataVideoEmbed = node.GetAttributeValue("data-video-embed", "").Trim().NullIfUndefined(),
                Href = node.GetAttributeValue("href", "").Trim().NullIfUndefined(),
                Id = Guid.NewGuid().ToString().NullIfUndefined(),
                Name = node.Name.ToLower().NullIfUndefined(),
                Src = (node.GetAttributeValue("data-src", null) ?? node.GetAttributeValue("src", "")).NullIfUndefined(),
                Text = HttpUtility.HtmlDecode(node.InnerText).Trim().NullIfUndefined()
            };
        }

        private static string GetFirstHeadlineTitle(HtmlNode node)
        {
            HtmlNode firstHeadline = node.SelectSingleNode(".//h1 | .//h2 | .//h3 | .//h4 | .//h5 | .//h6");

            return firstHeadline != null
                ? HttpUtility.HtmlDecode(firstHeadline.InnerText)
                : null;
        }

        private static bool IsCardCollection(HtmlNode node)
        {
            return node.Name == "div" && node.HasClass("mc-tabbed-cards-center-grid");
        }

        private static bool IsCarousel(HtmlNode node)
        {
            return node.Name == "section" && node.HasClass("mc-content-card-carousel");
        }

        private static bool IsSection(HtmlNode node)
        {
            return (node.Name == "section" && node.Attributes["role"]?.Value != "tabpanel")
                || node.Attributes["class"]?.Value?.Contains("section") == true
                || node.Attributes["class"]?.Value?.Contains("row row-size1 row-bottom-size2") == true;
        }

        private static bool IsTabList(HtmlNode node)
        {
            return node.Attributes["role"]?.Value == "tablist";
        }

        private static bool IsTable(HtmlNode node)
        {
            return node.Name == "table";
        }

        private static bool IsInsideTable(HtmlNode node)
        {
            return node.ParentNode?.Name == "th"
                || node.ParentNode?.Name == "td"
                || (node.ParentNode != null && IsInsideTable(node.ParentNode));
        }

        private static bool IsNotEmpty(HtmlNode node)
        {
            switch (node.Name)
            {
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "p":
                    return HttpUtility.HtmlDecode(node.InnerText).Trim().IsDefined();
                case "img":
                    string src = node.GetAttributeValue("data-src", null) ?? node.GetAttributeValue("src", "");
                    string alt = node.GetAttributeValue("alt", "").Trim();
                    return src.IsDefined() || alt.IsDefined();
                case "a":
                    string href = node.GetAttributeValue("href", "").Trim();
                    string text = HttpUtility.HtmlDecode(node.InnerText);
                    return (href.IsDefined() && !href.StartsWith("javascript")) || text.IsDefined();
                case "button":
                    return HttpUtility.HtmlDecode(node.InnerText).Trim().IsDefined()
                        || node.GetAttributeValue("data-video-embed", "").Trim().IsDefined();
                case "li" when node.InnerText.IsDefined():
                case "div" when node.InnerText == node.InnerHtml && node.InnerText.IsDefined():
                case "span" when node.InnerText == node.InnerHtml && node.InnerText.IsDefined():
                case "iframe":
                    return true;
            }

            return false;
        }

        private static bool IsNotEmptyCell(HtmlNode node)
        {
            return node.InnerText.IsDefined();
        }
    }
}
