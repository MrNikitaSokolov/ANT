using System;

namespace WebCrawler
{
    class WebCrawler
    {
        static void Main(string[] args)
        {
            var manager = new CrawlManager(
                seedUrls: new []{ "https://en.wikipedia.org/wiki/Tarjan's_strongly_connected_components_algorithm" },
                workerQueueSize: 50,
                numberOfWorkers: 4,
                maximumNodesCount: 1000);

            manager.StartCrawl();

            while (true)
            {
            }
        }
    }
}
