using System.Collections.Generic;

namespace WebCrawler
{
    /// <summary>
    /// Node in Web WebGraph
    /// </summary>
    public class WebGraphNode
    {
        public WebGraphNode(
            string url,
            string parent = null)
        {
            Children = new HashSet<string>();
            Parents = !string.IsNullOrEmpty(parent)
                ? new HashSet<string>(new[] {parent})
                : new HashSet<string>();
            Url = url;
        }

        /// <summary>
        /// Add child to the node
        /// </summary>
        public void AddChild(string child)
        {
            Children.Add(child);
        }

        /// <summary>
        /// Add parent to the node
        /// </summary>
        public void AddParent(string parent)
        {
            Parents.Add(parent);
        }

        /// <summary>
        /// Mark the node as processed
        /// </summary>
        public void MarkAsProcessed()
        {
            IsProcessed = true;
        }

        /// <summary>
        /// Url, encapsulated in the node
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Children of node
        /// </summary>
        public HashSet<string> Children { get; }

        /// <summary>
        /// Parents of node
        /// </summary>
        public HashSet<string> Parents { get; }

        /// <summary>
        /// Flag to show if the search was performed from this node
        /// </summary>
        public bool IsProcessed { get; private set; }
    }
}
