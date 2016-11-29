using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace WebCrawler
{
    /// <summary>
    /// HtmlAgilityPack implementation of IHyperlinkFinder
    /// </summary>
    public class HtmlAgilityPackHyperlinkFinder : IHyperlinkFinder
    {
        public IReadOnlyCollection<string> GetHyperlinks(string pageContents)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(pageContents);

            return doc.DocumentNode
                .SelectNodes("//a[@href]")
                .Select(link => link.Attributes["href"])
                .Select(att => att.Value)
                .ToArray();
        }
    }
}
