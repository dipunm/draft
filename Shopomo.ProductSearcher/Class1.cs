using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopomo.ProductSearcher
{

    public interface IProductSearcher
    {
        Task<ISearchResult> SearchAsync(SearchModel query, IEnumerable<IHintRequest<object>> supportingRequests);
    }

    public interface ISearchResult
    {
        IEnumerable<ProductSummary> Products { get; }
        int Total { get; }

        TOut Get<TRequest, TOut>() where TRequest : IHintRequest<TOut>;
    }

    public interface IHintRequest<out TOut>
    {

    }

    public abstract class FilterOptions
    {
        protected FilterOptions(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; }
    }

    public class Departments : FilterOptions, IHintRequest<string[]>
    {
        public Departments(int limit) : base(limit)
        {
        }
    }

    public class RelatedRetailers : FilterOptions, IHintRequest<string[]>
    {
        public RelatedRetailers(int limit) : base(limit)
        {
        }
    }

    public class RelatedBrands : FilterOptions, IHintRequest<string[]>
    {
        public RelatedBrands(int limit) : base(limit)
        {
        }
    }

    public class SpellingSuggestion : IHintRequest<string>
    {

    }

    public class ProductSummary
    {
        public string Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string ImageUrl { get; }
        public bool OnSale { get; }
        public bool FreeDelivery { get; }
        public int Rating { get; }
        public decimal OldPrice { get; }
    }


}
