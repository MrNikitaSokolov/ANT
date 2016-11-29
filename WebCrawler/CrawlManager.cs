using System.Linq;
using System.Threading;

namespace WebCrawler
{
    /// <summary>
    /// Manager of threads, which are responsible for crawling
    /// </summary>
    public class CrawlManager
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="numberOfWorkers">Number of working crawlers</param>
        public CrawlManager(int numberOfWorkers)
        {
            _workers = Enumerable.Repeat(
                new CrawlWorker(
                    new HtmlAgilityPackHyperlinkFinder(),
                    new DefaultPageDownload()),
                numberOfWorkers).ToArray();
            _cancelToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Issue an order to all workers to start crawling
        /// </summary>
        public void StartCrawl()
        {
            foreach (var worker in _workers)
            {
                // To support restart after stopping
                _cancelToken = new CancellationTokenSource();

                worker.Crawl(_cancelToken.Token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Issue and order to all workers to stop crawling
        /// </summary>
        public void StopCrawl()
        {
            _cancelToken.Cancel();
        }

        private readonly CrawlWorker[] _workers;
        private CancellationTokenSource _cancelToken;
    }
}
