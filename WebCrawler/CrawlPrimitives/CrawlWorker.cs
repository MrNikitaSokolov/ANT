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
            WebGraph graph,
            int queueSize,
            IHyperlinkFinder hyperlinkFinder,
            IPageDownload pageDownloader)
        {
            _graph = graph;
            _queueSize = queueSize;
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

                    await crawlIteration().ConfigureAwait(false);
                }
            }, token).ConfigureAwait(false);
        }

        private async Task crawlIteration()
        {
            string link;
            if (tryGetNewLink(out link))
            {
                var pageContents = await _pageDownloader.GetPageContentsAsync(link).ConfigureAwait(false);
                var foundLinks = _hyperlinkFinder.GetHyperlinks(link, pageContents);
                _graph.AddNodes(link, foundLinks);
            }
        }

        private bool tryGetNewLink(out string link)
        {
            if (_linksQueue.Any())
            {
                link = _linksQueue.Dequeue();
                return true;
            }

            link = null;
            var newLinks = _graph.GetLinksBatch(_queueSize);
            foreach (var newLink in newLinks)
            {
                _linksQueue.Enqueue(newLink);
            }
            return false;
        }

        private readonly WebGraph _graph;
        private readonly int _queueSize;
        private readonly IHyperlinkFinder _hyperlinkFinder;
        private readonly IPageDownload _pageDownloader;
        private readonly Queue<string> _linksQueue;
    }
}
