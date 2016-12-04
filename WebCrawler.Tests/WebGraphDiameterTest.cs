using NUnit.Framework;

namespace WebCrawler.Tests
{
    [TestFixture]
    public class WebGraphDiameterTest
    {
        [Test]
        public void GetDiameter_ValidGraph_ReturnCorrectDiameter()
        {
            var graph = new WebGraph(new [] {"1"});
            graph.AddNodes("1", new []{"2"});
            graph.AddNodes("2", new[] { "3" });
            graph.AddNodes("3", new[] { "5", "4" });
            graph.AddNodes("5", new[] { "6" });

            var floydWarshallAlg = new FloydWarshallDiameter();
            Assert.AreEqual(4, floydWarshallAlg.GetDiameter(graph));
        }
    }
}
