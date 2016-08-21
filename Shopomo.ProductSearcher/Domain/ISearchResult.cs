using System.Collections.Generic;
using Shopomo.ProductSearcher.Domain.AdditionalData;

namespace Shopomo.ProductSearcher.Domain
{
    public interface ISearchResult<out T>
    {
        IEnumerable<T> Products { get; }
        int Total { get; }

        TOut Get<TRequest, TOut>() where TRequest : IAdditionalData<TOut>;
    }
}