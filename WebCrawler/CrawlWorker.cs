using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebCrawler
{
    /// <summary>
    /// Worker performing simple crawling
    /// </summary>
    public class CrawlWorker
    {
        public CrawlWorker(
            IHyperlinkFinder hyperlinkFinder,
            IPageDownload pageDownloader)
        {
            _hyperlinkFinder = hyperlinkFinder;
            _pageDownloader = pageDownloader;
            _linksQueue = new Queue<string>();
        }

        /// <summary>
        /// Start crawling on a separate CLR thread
        /// </summary>
        public async Task Crawl(CancellationToken token)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                        return;
                    await process().ConfigureAwait(false);
                }
            }, token).ConfigureAwait(false);
        }

        private async Task process()
        {
            string link = _linksQueue.Dequeue();

            string pageContents = await _pageDownloader.GetPageContentsAsync(link);

            string[] foundLinks = _hyperlinkFinder.GetHyperlinks(pageContents).ToArray();


        }

        private readonly IHyperlinkFinder _hyperlinkFinder;
        private readonly IPageDownload _pageDownloader;
        private readonly Queue<string> _linksQueue;
    }
}
