using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler
{
    /// <summary>
    /// Web graph
    /// </summary>
    public class WebGraph
    {
        public WebGraph(IReadOnlyCollection<string> seedUrls)
        {
            NodesByUrl = seedUrls
                .Select(url => new WebGraphNode(url))
                .ToDictionary(n => n.Url);
        }

        /// <summary>
        /// Event notifying on a node being added to the graph
        /// </summary>
        public event NodeAddedEventHandler NodeAdded;
        public delegate void NodeAddedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Add nodes to the web graph
        /// </summary>
        /// <param name="parentUrl">Url of the parent</param>
        /// <param name="childUrls">Child urls of the node</param>
        public void AddNodes(string parentUrl, IReadOnlyCollection<string> childUrls)
        {
            lock (_obj)
            {
                WebGraphNode parentNode;
                // Parent must be in a graph
                if (!NodesByUrl.TryGetValue(parentUrl, out parentNode))
                    throw new InvalidOperationException();

                parentNode.MarkAsProcessed();

                foreach (var childUrl in childUrls)
                {
                    parentNode.AddChild(childUrl);

                    WebGraphNode foundNode;
                    if (NodesByUrl.TryGetValue(childUrl, out foundNode))
                    {
                        foundNode.AddParent(parentUrl);
                    }
                    else
                    {
                        NodesByUrl.Add(childUrl, new WebGraphNode(childUrl, parentUrl));
                        NodeAdded?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public Dictionary<string, WebGraphNode> NodesByUrl { get; }

        /// <summary>
        /// Get a batch of links of unprocessed nodes
        /// </summary>
        public IReadOnlyCollection<string> GetLinksBatch(int batchSize)
        {
            lock (_obj)
            {
                return NodesByUrl.Values
                    .Where(n => !n.IsProcessed)
                    .Take(batchSize)
                    .Select(n => n.Url)
                    .ToArray();
            }
        }

        private static readonly object _obj = new object();
    }
}
