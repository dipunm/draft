using System.Collections.Generic;
using System.Threading.Tasks;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using Shopomo.ProductSearcher.Domain.Projections;
using Shopomo.ProductSearcher.Domain.Search;

namespace Shopomo.ProductSearcher.Domain
{
    public interface IProductSearcher
    {
        Task<ISearchResult<ProductSummary>> SearchAsync(SearchModel search,
            ICollection<IAdditionalData<object>> interests);
    }
}