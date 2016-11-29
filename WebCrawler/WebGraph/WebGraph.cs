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
            _nodesByUrl = seedUrls
                .Select(url => new WebGraphNode(url))
                .ToDictionary(n => n.Url);
        }

        /// <summary>
        /// Add node to the web graph
        /// </summary>
        /// <param name="parentUrl">Url of the parent</param>
        /// <param name="url">Url of the node</param>
        public void AddNode(string parentUrl, string url)
        {
            WebGraphNode foundNode;
            if (_nodesByUrl.TryGetValue(url, out foundNode))
            {
                foundNode.AddParent(parentUrl);
            }
            else
            {
                _nodesByUrl.Add(url, new WebGraphNode(url, parentUrl));
            }

            // Parent must be in a graph
            if (!_nodesByUrl.TryGetValue(parentUrl, out foundNode))
                throw new InvalidOperationException();
            foundNode.AddChild(url);
        }

        /// <summary>
        /// Get all nodes of web graph
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<WebGraphNode> GetAllNodes()
        {
            return _nodesByUrl.Values.ToArray();
        }

        private readonly Dictionary<string, WebGraphNode> _nodesByUrl;
    }
}
