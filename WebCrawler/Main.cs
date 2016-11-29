using System;
using System.Threading.Tasks;

namespace WebCrawler
{
    class WebCrawler
    {
        static void Main(string[] args)
        {
            var manager = new CrawlManager(1);
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
