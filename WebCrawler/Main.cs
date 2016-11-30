using System;
using System.Threading.Tasks;

namespace WebCrawler
{
    class WebCrawler
    {
        static void Main(string[] args)
        {
            var manager = new CrawlManager(
                seedUrls: new []{ "https://en.wikipedia.org/wiki/Main_Page" },
                workerQueueSize: 5,
                numberOfWorkers: 10);

            manager.StartCrawl();
            while (true)
            {
                if (Console.ReadLine() == "q")
                    manager.StopCrawl();
                if (Console.ReadLine() == "s")
                    manager.StartCrawl();
            }
        }
    }
}
