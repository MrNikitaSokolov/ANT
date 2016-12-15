using System;
using static System.Int32;

namespace WebCrawler
{
    class WebCrawler
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide at least one seed URL as a command line argument.");
                return;
            }

            Console.WriteLine("ANT (Analyzed Node Traversal) by Nikita Sokolov © December 2016");
            Console.WriteLine("Project for CS9668 Internet Algorithmics.");

            Console.Write("Put worker queue size: ");
            var workerQueueSize = Parse(Console.ReadLine());
            
            Console.Write("Put number of workers: ");
            var numberOfWorkers = Parse(Console.ReadLine());

            Console.Write("Put the number of nodes: ");
            var nodesCount = Parse(Console.ReadLine());

            var manager = new CrawlManager(args, numberOfWorkers, workerQueueSize, nodesCount);
            manager.StartCrawl();
            while (true)
            {
            }
        }
    }
}
