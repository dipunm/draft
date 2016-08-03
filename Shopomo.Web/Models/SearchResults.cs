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
    //    public SearchResults(ProductSearchResults results)
    //    {
    //        try
    //        {
    //            Products = results.GetProducts().SimplifyForListing();
    //            Departments = results.GetDepartments();
    //            Brands = results.GetFilters("brands");
    //            Retailers = results.GetFilters("retailers");
    //            Count = results.GetProducts().Count();
    //            DidYouMean = results.GetSpellingSuggestion();
    //        }
    //        catch (Exception e)
    //        {
                
    //        }
    //    }


        public string DidYouMean { get; set; }

        public IEnumerable<object> Products { get; set; }

        public IEnumerable<object> DepartmentFilters { get; set; }

        public IEnumerable<object> BrandFilters { get; set; }

        public IEnumerable<object> RetailerFilters { get; set; }

        public int Total { get; set; }
    }
}