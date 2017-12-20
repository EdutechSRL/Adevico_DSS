    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    namespace lm.Comol.Core.DomainModel.Helpers
    {
        public class HtmlSanitizer
        {
            public HtmlSanitizer()
            {
                HtmlNode.ElementsFlags.Remove("form");
            }

            public HashSet<string> BlackList = new HashSet<string>() 
        {
                { "script" },
                { "iframe" },                
                { "object" },
                { "embed" },
                { "link" }
        };

            public static string SanitizeHtml(string html, params string[] blackList)
            {
                var sanitizer = new HtmlSanitizer();
                if (blackList != null && blackList.Length > 0)
                {
                    sanitizer.BlackList.Clear();
                    foreach (string item in blackList)
                        sanitizer.BlackList.Add(item);
                }
                return sanitizer.Sanitize(html);
            }

            public string Sanitize(string html)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                SanitizeHtmlNode(doc.DocumentNode);
                return doc.DocumentNode.WriteTo();
            }

            private void SanitizeHtmlNode(HtmlNode node)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    // check for blacklist items and remove
                    if (BlackList.Contains(node.Name))
                    {
                        node.Remove();
                        return;
                    }
                }

                // Look through child nodes recursively
                if (node.HasChildNodes)
                {
                    for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
                    {
                        SanitizeHtmlNode(node.ChildNodes[i]);
                    }
                }
            }

        }
    }