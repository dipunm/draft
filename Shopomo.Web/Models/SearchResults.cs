using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopomo.Searchers;
using Shopomo.Web.Controllers.Api;

namespace Shopomo.Web.Models
{
    public class SearchResults
    {
        public SearchResults(ProductSearchResults results, SearchModel userQuery)
        {
            try
            {
                UserQuery = userQuery;
                Products = results.GetProducts().SimplifyForListing();
                Departments = results.GetDepartments();
                Brands = results.GetFilters("brands");
                Retailers = results.GetFilters("retailers");
                Count = results.GetProducts().Count();
                DidYouMean = results.GetSpellingSuggestion();
            }
            catch (Exception e)
            {
                
            }
        }

   
        public string DidYouMean { get; set; }

        public SearchModel UserQuery { get; set; }

        public IEnumerable<object> Products { get; set; }

        public IEnumerable<object> Departments { get; set; }

        public IEnumerable<object> Brands { get; set; }

        public IEnumerable<object> Retailers { get; set; }

        public int Count { get; set; }
    }
}