using System;
using System.Collections.Generic;
using System.Linq;
using ReturnNull.ValueProviders;
using ReturnNull.ValueProviders.Web.ModelBinding;
using Shopomo.ProductSearcher;
using Shopomo.ProductSearcher.Domain;

namespace Shopomo.Web.Models.Binders
{
    public class SearchModelBuilder : IModelBuilder<SearchModel>
    {
        private const int MaxPageSize = 100;

        public SearchModel BuildModel(IValueProvider dataProvider)
        {
            dataProvider = dataProvider.LimitedTo("querystring");

            var query = dataProvider.GetValue<string>("q");
            var filters = new SearchFilters()
            {
                Brands = dataProvider.GetValues<string>("brands"),
                Department = dataProvider.GetValue<string>("department"),
                Retailers = dataProvider.GetValues<string>("retailer"),
                WithFreeDelivery = dataProvider.GetValue<bool?>("freedelivery"),
                OnSale = dataProvider.GetValue<bool?>("onsale"),
                PriceRange = new PriceRange()
                {
                    Max = dataProvider.GetValue<decimal?>("maxprice"),
                    Min = dataProvider.GetValue<decimal?>("minprice")
                }
            };
            return new SearchModel()
            {
                Query = query,
                Filters = filters,
                Page = new PageModel()
                {
                    Size = Math.Min(Math.Abs(dataProvider.GetValue("pagesize", defaultValue: 9)), MaxPageSize),
                    Start = Math.Abs(dataProvider.GetValue("pagestart", defaultValue: 0))
                },
                Order = CalculateSort(dataProvider.GetValue<string>("sort"), query, filters)
            };
        }

        private Sort CalculateSort(string sort, string query, SearchFilters filters)
        {
            switch (sort)
            {
                case "priceasc":
                    return Sort.PriceAsc;
                case "pricedesc":
                    return Sort.PriceDesc;
                default:
                    if (string.IsNullOrEmpty(query))
                    {
                        return string.IsNullOrEmpty(filters.Department) ? Sort.RandomOrder : Sort.PriorityThenRandom;
                    }
                    else
                    {
                        return Sort.Relevance;
                    }

            }
        }
    }
}