using System.Linq;
using NUnit.Framework;

namespace WebCrawler.Tests
{
    [TestFixture]
    public class HyperlinkFinderTest
    {
        [Test]
        public void GetHyperlinks_SimplePage_ReturnCorrectLinks()
        {
            var finder = new HtmlAgilityPackHyperlinkFinder();
            var contents = "<!DOCTYPE html>" +
                           "<html>" +
                           "<body>" +
                           "<p><a href=\"http://www.w3schools.com/html/\">Visit our HTML tutorial</a></p>" +
                           "</body>" +
                           "</html>";
            var result = finder.GetHyperlinks("http://www.w3schools.com", contents).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("http://www.w3schools.com/html/", result[0]);
        }

        [Test]
        public void GetHyperlinks_EmptyContents_ReturnsEmptyList()
        {
            var finder = new HtmlAgilityPackHyperlinkFinder();
            var result = finder.GetHyperlinks("test", "");
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetHyperlinks_PageWithNoLinks_ReturnsEmptyList()
        {
            var finder = new HtmlAgilityPackHyperlinkFinder();
            var contents = "<!DOCTYPE html>" +
                           "<html>" +
                           "<body>" +
                           "<p>Visit our HTML tutorial</a></p>" +
                           "</body>" +
                           "</html>";
            var result = finder.GetHyperlinks("http://www.w3schools.com", contents).ToArray();
            Assert.IsEmpty(result);
        }
    }
}
