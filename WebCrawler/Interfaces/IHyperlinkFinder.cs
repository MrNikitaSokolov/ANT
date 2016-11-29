using System.Collections.Generic;

namespace WebCrawler
{
    /// <summary>
    /// Interface for finding hyperlinks in page
    /// </summary>
    public interface IHyperlinkFinder
    {
        /// <summary>
        /// Get all hyperlinks in page
        /// </summary>
        /// <param name="pageContents">Contents of the page to search for hyperlinks</param>
        IReadOnlyCollection<string> GetHyperlinks(string pageContents);
    }
}
