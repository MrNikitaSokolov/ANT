using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler
{
    public class TarjanStronglyConnectedComponents : IStronglyConnectedComponentsFinder
    {
        public IReadOnlyCollection<IReadOnlyCollection<WebGraphNode>> GetStronglyConnectedComponents(WebGraph graph)
        {
            var dict = graph.NodesByUrl.Values.ToDictionary(n => n.Url, n => new SpecialNode(n));
            foreach (var node in dict.Values)
            {
                if (node.Index == null)
                    strongConnect(dict, node);
            }
            return _scc;
        }

        private void strongConnect(Dictionary<string, SpecialNode> nodesByUrl, SpecialNode node)
        {
            node.Index = _index;
            node.Node.StronglyConnectedComponentIndex = _index;
            _index++;
            _stack.Push(node);
            node.OnStack = true;

            foreach (var child in node.Node.Children)
            {
                var childNode = nodesByUrl[child];
                if (childNode.Index == null)
                {
                    strongConnect(nodesByUrl, childNode);
                    node.Node.StronglyConnectedComponentIndex = Math.Min(
                        node.Node.StronglyConnectedComponentIndex.Value, childNode.Node.StronglyConnectedComponentIndex.Value);
                }
                else if (childNode.OnStack)
                    node.Node.StronglyConnectedComponentIndex = Math.Min(
                        node.Node.StronglyConnectedComponentIndex.Value, childNode.Index.Value);
            }

            if (node.Node.StronglyConnectedComponentIndex == node.Index)
            {
                var group = new List<WebGraphNode>();
                SpecialNode w;
                do
                {
                    w = _stack.Pop();
                    group.Add(w.Node);
                } while (!string.Equals(w.Node.Url, node.Node.Url, StringComparison.InvariantCultureIgnoreCase));
                _scc.Add(group.ToArray());
            }
        }

        private class SpecialNode
        {
            public SpecialNode(WebGraphNode node)
            {
                Node = node;
            }
            public WebGraphNode Node { get; }
            public bool OnStack { get; set; }
            public int? Index { get; set; }
        }

        private int _index;
        private readonly List<WebGraphNode[]> _scc = new List<WebGraphNode[]>();
        private readonly Stack<SpecialNode> _stack = new Stack<SpecialNode>();
    }
}
