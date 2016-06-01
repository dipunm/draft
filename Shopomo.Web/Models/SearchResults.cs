using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopomo.Web.Controllers.Api;

namespace Shopomo.Web.Models
{
    public class FirstSearchResult : SearchResults
    {
        public FirstSearchResult(ProductSearchResults results, SearchModel userQuery) 
            : base(results, userQuery)
        {
            //DidYouMean = results.GetSpellingSuggestion();
        }

        public string DidYouMean { get; set; }
    }

    public class PagedSearchResults : SearchResults
    {
        public PagedSearchResults(ProductSearchResults results, SearchModel userQuery) 
            : base(results, userQuery)
        { }
    }

    public abstract class SearchResults
    {
        protected SearchResults(ProductSearchResults results, SearchModel userQuery)
        {
            try
            {
                UserQuery = userQuery;
                Products = results.GetProducts().SimplifyForListing();
                Departments = results.GetDepartments();
                Brands = results.GetFilters("brands");
                Retailers = results.GetFilters("retailers");
                Count = results.GetProducts().Count();
            }
            catch (Exception e)
            {
                
            }
        }

        public SearchModel UserQuery { get; set; }

        public IEnumerable<object> Products { get; set; }

        public IEnumerable<object> Departments { get; set; }

        public IEnumerable<object> Brands { get; set; }

        public IEnumerable<object> Retailers { get; set; }

        public int Count { get; set; }
    }
}