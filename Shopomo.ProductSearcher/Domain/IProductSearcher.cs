using System.Collections.Generic;
using System.Threading.Tasks;
using Shopomo.ProductSearcher.Domain.Projections;
using Shopomo.ProductSearcher.Domain.Search;

namespace Shopomo.ProductSearcher.Domain
{

    public interface IProductSearcher
    {
        Task<ISearchResult<ProductSummary>> SearchAsync(SearchModel query, IEnumerable<ISearchMeta<object>> interests);
    }

    public interface ISearchResult<out T>
    {
        IEnumerable<T> Products { get; }
        int Total { get; }

        TOut Get<TRequest, TOut>() where TRequest : ISearchMeta<TOut>;
    }

    public interface ISearchMeta<out TOut>
    {

    }
}
