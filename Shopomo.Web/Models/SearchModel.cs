using System.Collections.Generic;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Shopomo.Web.Models
{
    public class SearchModel
    {
        public string QueryText { get; set; }
        public string DepartmentId { get; set; }
        public IEnumerable<string> BrandIds { get; set; }
        public IEnumerable<string> RetailerIds { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public Sort SortBy { get; set; }
    }

    public enum Sort
    {
        Relevance = 0, PriceAsc, PriceDesc
    }
}