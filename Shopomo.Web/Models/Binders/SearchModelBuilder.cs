using System;
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

            return new SearchModel()
            {
                Query = dataProvider.GetValue<string>("q"),
                Filters = new SearchFilters()
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
                },
                Page = new PageModel()
                {
                    Size = Math.Min(Math.Abs(dataProvider.GetValue("pagesize", defaultValue: 9)), MaxPageSize),
                    Start = Math.Abs(dataProvider.GetValue("pagestart", defaultValue: 0))
                },
                Order = dataProvider.GetValue("sort", Sort.Relevance)
            };
        }
    }
}