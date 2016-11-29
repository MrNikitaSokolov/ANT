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
            var result = finder.GetHyperlinks(contents).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("http://www.w3schools.com/html/", result[0]);
        }
    }
}
