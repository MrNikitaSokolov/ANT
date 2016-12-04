using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;

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
        /// <param name="seedUrls">Seed urls to start crawling</param>
        /// <param name="numberOfWorkers">Number of working crawlers</param>
        /// <param name="workerQueueSize">Size of queue for every worker</param>
        /// <param name="maximumNodesCount">Maximum number of nodes in a graph</param>
        public CrawlManager(
            IReadOnlyCollection<string> seedUrls,
            int numberOfWorkers,
            int workerQueueSize,
            int maximumNodesCount)
        {
            _graph = new WebGraph(seedUrls);
            _workers = Enumerable.Repeat(
                new CrawlWorker(
                    _graph,
                    workerQueueSize,
                    new HtmlAgilityPackHyperlinkFinder(),
                    new DefaultPageDownload()),
                numberOfWorkers).ToArray();
            _maximumNodesCount = maximumNodesCount;

            _cancelToken = new CancellationTokenSource();
            _graph.NodeAdded += graphNodeAdded;
        }

        /// <summary>
        /// Issue an order to all workers to start crawling
        /// </summary>
        public void StartCrawl()
        {
            foreach (var worker in _workers)
            {
                worker.Crawl(_cancelToken.Token).ConfigureAwait(false);
            }
        }

        public void CalculateDiameter()
        {
            if (!_crawlCompleted)
                return;

            Console.WriteLine("Calculating diameter...");
            var floydWarshall = new FloydWarshallDiameter();
            var diameter = floydWarshall.GetDiameter(_graph);

            Console.WriteLine("Diameter is {0}.", diameter);
        }

        public void CalculateStronglyConnectedComponents(bool showConsoleInfo = true)
        {
            if (!_crawlCompleted)
                return;

            if (showConsoleInfo)
                Console.WriteLine("Calculating strongly connected components...");

            var tarjanAlgo = new TarjanStronglyConnectedComponents();
            var scc = tarjanAlgo.GetStronglyConnectedComponents(_graph);

            foreach (var group in scc.Where(c => c.Count <= 1))
            {
                _graph.NodesByUrl[group.Single().Url].StronglyConnectedComponentIndex = -1;
            }

            if (showConsoleInfo)
            {
                var groupByLowlink = _graph.NodesByUrl.Values.Where(n => n.StronglyConnectedComponentIndex != -1)
                .GroupBy(n => n.StronglyConnectedComponentIndex)
                .ToDictionary(item => item.Key, item => item.ToArray());
                var groupCounter = 1;
                foreach (var group in groupByLowlink)
                {
                    Console.WriteLine("Group {0}:", groupCounter++);
                    foreach (var node in group.Value)
                    {
                        Console.WriteLine(node.Url);
                    }
                }
            }
        }

        public void Visualize()
        {
            if (!_crawlCompleted)
                return;

            Console.WriteLine("Opening corresponding visualization...");
            CalculateStronglyConnectedComponents(false);

            createJsonRepresentation();
            System.Diagnostics.Process.Start(@"Visualization\Webgraph.html");
        }

        private void graphNodeAdded(object sender, EventArgs e)
        {
            if (_crawlCompleted)
                return;

            var graph = sender as WebGraph;
            if (graph == null)
                throw new InvalidOperationException();
            var numberOfCollectedNodes = graph.NodesByUrl.Values.Count;

            Console.Write("\r{0}", numberOfCollectedNodes + " processed.");
            if (numberOfCollectedNodes < _maximumNodesCount)
                return;

            _crawlCompleted = true;
            _cancelToken.Cancel();
            Console.Write("\r{0}", "Crawling is finished.");
        }

        private void createJsonRepresentation()
        {
            var nodes = new List<NodeRepresentation>();
            var edges = new List<EdgeRepresentation>();
            foreach (var node in _graph.NodesByUrl.Values)
            {
                nodes.Add(new NodeRepresentation
                {
                    Id = node.Url,
                    Label = node.Url,
                    Group = node.StronglyConnectedComponentIndex.ToString()
                });

                edges.AddRange(node.Children.Select(child => new EdgeRepresentation
                {
                    From = node.Url,
                    To = child
                }));
            }

            using (StreamWriter file = File.CreateText(@"Visualization\nodes.json"))
            {
                file.Write("nodes = ");
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, nodes);
            }

            using (StreamWriter file = File.CreateText(@"Visualization\edges.json"))
            {
                file.Write("edges = ");
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, edges);
            }
        }

        [DataContract]
        private class NodeRepresentation
        {
            [DataMember]
            [JsonProperty("id")]
            public string Id { get; set; }

            [DataMember]
            [JsonProperty("label")]
            public string Label { get; set; }

            [DataMember]
            [JsonProperty("group")]
            public string Group { get; set; }
        }

        [DataContract]
        public class EdgeRepresentation
        {
            public EdgeRepresentation()
            {
                Arrows = "to";
            }

            [DataMember]
            [JsonProperty("from")]
            public string From { get; set; }

            [DataMember]
            [JsonProperty("to")]
            public string To { get; set; }

            [DataMember]
            [JsonProperty("arrows")]
            public string Arrows { get; set; }

        }

        private readonly CrawlWorker[] _workers;
        private readonly CancellationTokenSource _cancelToken;
        private readonly int _maximumNodesCount;
        private readonly WebGraph _graph;
        private bool _crawlCompleted;
    }
}
