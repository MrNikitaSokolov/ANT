using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler
{
    public class TarjanStronglyConnectedComponents : IStronglyConnectedComponentsFinder
    {
        public IReadOnlyCollection<IReadOnlyCollection<WebGraphNode>> GetStronglyConnectedComponents(WebGraph graph)
        {
            foreach (var node in graph.NodesByUrl.Values.Select(n => new SpecialNode(n)))
            {
                if (node.Index == null)
                    strongConnect(graph.NodesByUrl.Values.ToDictionary(n => n.Url, n => new SpecialNode(n)), node);
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
                } while (w.Node.Url != node.Node.Url);
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
