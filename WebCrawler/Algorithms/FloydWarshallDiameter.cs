using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler
{
    public class FloydWarshallDiameter : IDiameterFinder
    {
        public int GetDiameter(WebGraph graph)
        {
            var nodesArray = graph.NodesByUrl.Values.ToArray();
            var nodesCount = nodesArray.Length;
            var adjacencyMatrix = getAdjacencyMatrix(graph);
            var diameter = 1;

            for (var k = 0; k < nodesCount; k++)
            for (var i = 0; i < nodesCount; i++)
            for (var j = 0; j < nodesCount; j++)
            {
                if (adjacencyMatrix[i, j] > adjacencyMatrix[i, k] + adjacencyMatrix[k, j])
                {
                    adjacencyMatrix[i, j] = Math.Min(MaxValue, adjacencyMatrix[i, k] + adjacencyMatrix[k, j]);
                    if (adjacencyMatrix[i, j] < MaxValue && adjacencyMatrix[i, j] > diameter)
                        diameter = adjacencyMatrix[i, j];
                }
            }

            return diameter;
        }

        private int[,] getAdjacencyMatrix(WebGraph graph)
        {
            var nodesArray = graph.NodesByUrl.Values.ToArray();
            var nodesCount = nodesArray.Length;

            var nodeById = new Dictionary<int, WebGraphNode>();
            for (var i = 0; i < nodesCount; i++)
            {
                nodeById.Add(i, nodesArray[i]);
            }

            var adjacencyMatrix = new int[nodesCount, nodesCount];
            for (var row = 0; row < nodesCount; row++)
            for (var column = 0; column < nodesCount; column++)
            {
                if (row == column)
                    adjacencyMatrix[row, column] = 0;
                else
                {
                    var node = nodeById[row];
                    var adjacentNode = nodeById[column];
                    if (node.Children.Contains(adjacentNode.Url))
                    {
                        adjacencyMatrix[row, column] = 1;
                    }
                    else
                    {
                        adjacencyMatrix[row, column] = MaxValue;
                    }
                }
             }
            return adjacencyMatrix;
        }

        private const int MaxValue = 1000;
    }
}
