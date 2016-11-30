using System.Linq;
using NUnit.Framework;

namespace WebCrawler.Tests
{
    [TestFixture]
    public class WebGraphTest
    {
        [Test]
        public void CreateWebGraph_OneSeedUrl_ReturnsOneNodeWithNoChildren()
        {
            var graph = new WebGraph(new[] {"url1"});
            var nodes = graph.GetAllNodes();
            Assert.AreEqual(1, nodes.Count);

            var singleNode = nodes.Single();
            Assert.IsEmpty(singleNode.Children);
            Assert.IsEmpty(singleNode.Parents);
        }

        [Test]
        public void AddNodes_OneChildNode_ReturnWebGraphWithTwoNodes()
        {
            var graph = new WebGraph(new[] { "url1" });
            graph.AddNodes("url1", new[] { "url2" });

            var nodes = graph.GetAllNodes();
            Assert.AreEqual(2, nodes.Count);

            var firstNode = nodes.Single(n => string.Equals(n.Url, "url1"));
            Assert.AreEqual(1, firstNode.Children.Count);
            Assert.IsEmpty(firstNode.Parents);
            Assert.IsTrue(firstNode.IsProcessed);

            var secondNode = nodes.Single(n => string.Equals(n.Url, "url2"));
            Assert.AreEqual(1, secondNode.Parents.Count);
            Assert.IsEmpty(secondNode.Children);
            Assert.IsFalse(secondNode.IsProcessed);
        }

        [Test]
        public void AddNodes_ChildNodePointBackToParent_ReturnsCorrectChildAndParents()
        {
            var graph = new WebGraph(new[] { "url1" });
            graph.AddNodes("url1", new[] {"url2"});
            graph.AddNodes("url2", new [] {"url1"});

            var nodes = graph.GetAllNodes();
            Assert.AreEqual(2, nodes.Count);

            var firstNode = nodes.Single(n => string.Equals(n.Url, "url1"));
            Assert.AreEqual(1, firstNode.Children.Count);
            Assert.AreEqual(1, firstNode.Parents.Count);
            Assert.IsTrue(firstNode.IsProcessed);

            var secondNode = nodes.Single(n => string.Equals(n.Url, "url2"));
            Assert.AreEqual(1, secondNode.Parents.Count);
            Assert.AreEqual(1, secondNode.Children.Count);
            Assert.IsTrue(secondNode.IsProcessed);
        }
    }
}
