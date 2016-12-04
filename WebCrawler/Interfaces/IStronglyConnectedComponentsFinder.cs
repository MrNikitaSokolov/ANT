using System.Collections.Generic;

namespace WebCrawler
{
    public interface IStronglyConnectedComponentsFinder
    {
        IReadOnlyCollection<IReadOnlyCollection<WebGraphNode>> GetStronglyConnectedComponents(WebGraph graph);
    }
}
