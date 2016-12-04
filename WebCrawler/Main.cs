using System;
using static System.Int32;

namespace WebCrawler
{
    class WebCrawler
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ANT (Analyzed Node Traversal) by Nikita Sokolov © December 2016");
            Console.WriteLine("Project for CS9668 Internet Algorithmics.");
            Console.WriteLine("Commands available:");
            Console.WriteLine("-s to start crawling;");
            Console.WriteLine("-d to find diameter after crawling is completed;");
            Console.WriteLine("-scc to find strongly connected components;");
            Console.WriteLine("-v to create visualization of webgraph.");

            Console.WriteLine("Put worker queue size: ");
            var workerQueueSize = Parse(Console.ReadLine());
            
            Console.WriteLine("Put number of workers: ");
            var numberOfWorkerks = Parse(Console.ReadLine());

            Console.WriteLine("Put the number of nodes: ");
            var nodesCount = Parse(Console.ReadLine());

            var manager = new CrawlManager(args, workerQueueSize, numberOfWorkerks, nodesCount);
            while (true)
            {
                var command = Console.ReadLine();
                switch (command)
                {
                    case "-s":
                        manager.StartCrawl();
                        break;
                    case "-d":
                        manager.CalculateDiameter();
                        break;
                    case "-scc":
                        manager.CalculateStronglyConnectedComponents();
                        break;
                    case "-v":
                        manager.Visualize();
                        break;
                }
            }
        }
    }
}
