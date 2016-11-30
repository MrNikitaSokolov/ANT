using System;
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
        public IReadOnlyCollection<string> GetHyperlinks(string baseUrl, string pageContents)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(pageContents);

            var linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
            if (linkNodes == null)
                return new string[0];

            return linkNodes
                    .Select(link => link.Attributes["href"])
                    .Select(att => getAbsoluteUrlString(baseUrl, att.Value))
                    .ToArray();
        }

        private static string getAbsoluteUrlString(string baseUrl, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);
            return uri.ToString();
        }
    }
}
