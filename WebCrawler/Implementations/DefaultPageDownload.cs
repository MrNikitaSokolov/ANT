using System.Net;
using System.Threading.Tasks;

namespace WebCrawler
{
    /// <summary>
    /// Default implementation of IPageDownload
    /// </summary>
    class DefaultPageDownload : IPageDownload
    {
        public async Task<string> GetPageContentsAsync(string url)
        {
            return await _webClient.DownloadStringTaskAsync(url).ConfigureAwait(false);
        }

        private static readonly WebClient _webClient = new WebClient();
    }
}
