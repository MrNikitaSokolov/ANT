using System.Collections.Generic;
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
                    await process().ConfigureAwait(false);
                }
            }, token).ConfigureAwait(false);
        }

        private async Task process()
        {
            if (_linksQueue.Count > 0)
            {
                var link = _linksQueue.Dequeue();
                var pageContents = await _pageDownloader.GetPageContentsAsync(link).ConfigureAwait(false);
                var foundLinks = _hyperlinkFinder.GetHyperlinks(link, pageContents);
                _graph.AddNodes(link, foundLinks);
            }
            else
            {
                var newLinks = _graph.GetLinksBatch(_queueSize);
                foreach (var newLink in newLinks)
                {
                    _linksQueue.Enqueue(newLink);
                }
            }
        }

        private readonly WebGraph _graph;
        private readonly int _queueSize;
        private readonly IHyperlinkFinder _hyperlinkFinder;
        private readonly IPageDownload _pageDownloader;
        private readonly Queue<string> _linksQueue;
    }
}
