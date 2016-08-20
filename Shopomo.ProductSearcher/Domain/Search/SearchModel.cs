using System.Threading;

namespace Shopomo.ProductSearcher.Domain.Search
{
    public class SearchModel
    {
        public SearchModel()
        {
            Filters = new SearchFilters();
            Page = new PageModel();
        }
        public string Query { get; set; }
        public SearchFilters Filters { get; }
        public Sort Order { get; set; }
        public PageModel Page { get; }
    }
}