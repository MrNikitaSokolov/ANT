using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebCrawler.Tests
{
    [TestFixture]
    public class PageDownloadTest
    {
        [Test]
        public void GetPageContentsAsync_NonExistentPage_DoesntThrowAnException()
        {
            var linkName = Guid.NewGuid().ToString();
            var pageDownloader = new DefaultPageDownload();
            Assert.DoesNotThrowAsync(async () =>
                await pageDownloader.GetPageContentsAsync(linkName).ConfigureAwait(false));
        }

        [Test]
        public async Task GetPageContentsAsync_NonExistentPage_ReturnsEmptyContent()
        {
            var linkName = Guid.NewGuid().ToString();
            var pageDownloader = new DefaultPageDownload();
            var result = await pageDownloader.GetPageContentsAsync(linkName).ConfigureAwait(false);
            Assert.AreEqual(string.Empty, result);
        }
    }
}
