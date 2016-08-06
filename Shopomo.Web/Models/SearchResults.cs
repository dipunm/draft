﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopomo.Searchers;
using Shopomo.Web.Controllers.Api;

namespace Shopomo.Web.Models
{
    public class SearchResults
    {
        public string DidYouMean { get; set; }

        public IEnumerable<object> Products { get; set; }

        public IEnumerable<object> DepartmentFilters { get; set; }

        public IEnumerable<object> BrandFilters { get; set; }

        public IEnumerable<object> RetailerFilters { get; set; }

        public int Total { get; set; }
    }

    public class ProductSearchData
    {
        public IEnumerable Products { get; }
        public int Total { get; }

        public IEnumerable QueryModifiers { get; }

            
            //facets, spelling, suggester
    }
    
}