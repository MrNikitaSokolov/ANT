using System;
using System.Net;
using System.Threading.Tasks;

namespace WebCrawler
{
    /// <summary>
    /// Default implementation of IPageDownload
    /// </summary>
    public class DefaultPageDownload : IPageDownload
    {
        public async Task<string> GetPageContentsAsync(string url)
        {
            using (var webClient = new WebClient())
            try
            {
                return await webClient.DownloadStringTaskAsync(url).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
