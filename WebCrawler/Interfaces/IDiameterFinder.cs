namespace WebCrawler
{
    public interface IDiameterFinder
    {
        /// <summary>
        /// Get the diameter of graph
        /// </summary>
        int GetDiameter(WebGraph graph);
    }
}
