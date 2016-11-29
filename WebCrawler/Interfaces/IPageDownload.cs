using System.Threading.Tasks;

namespace WebCrawler
{
    /// <summary>
    /// Interface for downloading pages from the web
    /// </summary>
    public interface IPageDownload
    {
        /// <summary>
        /// Asynchronously get page contents in a string representation
        /// </summary>
        /// <param name="url">Url to download a page from</param>
        Task<string> GetPageContentsAsync(string url);
    }
}
