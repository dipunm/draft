using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopomo.Web.Controllers.Api;

namespace Shopomo.Web.Models
{
    public class FirstSearchResult : SearchResults
    {
        public FirstSearchResult(ProductSearchResults results) : base(results)
        {
            DidYouMean = results.GetSpellingSuggestion();
        }

        public string DidYouMean { get; set; }
    }

    public class PagedSearchResults : SearchResults
    {
        public PagedSearchResults(ProductSearchResults results) : base(results)
        { }
    }

    public abstract class SearchResults
    {
        protected SearchResults(ProductSearchResults results)
        {
            Products = results.GetProducts().SimplifyForListing();
            Departments = results.GetDepartments();
            Brands = results.GetFilters("brands");
            Retailers = results.GetFilters("retailers");
            Count = results.GetProducts().Count();
            /*
            var products = results.GetProducts().SimplifyForListing();
            var departments = results.GetDepartments();
            var brands = results.GetFilters("brands");
            var retailers = results.GetFilters("retailers");
            //not required for api.
            //var didYouMean = await search.GetSpellingSuggestion();

            var me = new
            {
                //didYouMean,
                products,
                filters = new
                {
                    departments,
                    brands,
                    retailers,
                    userSearch.MinPrice,
                    userSearch.MaxPrice
                },
                Count = products.Count()
            };
            */
        }
    }
}