using System;

namespace WebCrawler
{
    /// <summary>
    /// Event, encapluslating a number of nodes in a webgraph
    /// </summary>
    public class WebGraphNodesCount : EventArgs
    {
        public WebGraphNodesCount(int numberOfNodes)
        {
            NumberOfNodes = numberOfNodes;
        }

        /// <summary>
        /// Current number of nodes in a webgraph
        /// </summary>
        public int NumberOfNodes { get; }
    }
}
