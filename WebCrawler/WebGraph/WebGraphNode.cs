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
        /// Url
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// 
        /// </summary>
        public HashSet<string> Children { get; }

        public HashSet<string> Parents { get; }
    }
}
