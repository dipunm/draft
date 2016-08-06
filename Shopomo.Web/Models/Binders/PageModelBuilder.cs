using System;
using System.Linq;
using ReturnNull.ValueProviders;
using ReturnNull.ValueProviders.Web.ModelBinding;

namespace Shopomo.Web.Models.Binders
{
    public class PageModelBuilder : IModelBuilder<PageModel>
    {
        private const int MaxPageSize = 100;
        public PageModel BuildModel(IValueProvider dataProvider)
        {
            dataProvider = dataProvider.LimitedTo("querystring");
            return new PageModel()
            {
                Size = Math.Min(Math.Abs(dataProvider.GetValue("pagesize", defaultValue: 9)), MaxPageSize),
                Start = dataProvider.GetValue("pagestart", defaultValue: 0)
            };
        }
    }

    public class SearchModelBuilder : IModelBuilder<SearchModel>
    {
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
                }
            };
        }
    }
}