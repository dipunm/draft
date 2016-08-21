using Library.Core;
using ReturnNull.ValueProviders;
using ReturnNull.ValueProviders.Web.ModelBinding;
using Shopomo.ProductSearcher.Domain.Search;

namespace Shopomo.Web.Models.Binders
{
    public class SearchModelBuilder : IModelBuilder<SearchModel>
    {
        public SearchModel BuildModel(IValueProvider dataProvider)
        {
            dataProvider = dataProvider.LimitedTo("querystring");

            var model = new SearchModel();
            model.Query = dataProvider.GetValue<string>("q");
            model.Filters.Department = dataProvider.GetValue<string>("department");
            model.Filters.WithFreeDelivery = dataProvider.GetValue<bool?>("freedelivery");
            model.Filters.Sale = dataProvider.GetValue<string>("sale");
            model.Filters.PriceRange = PriceRange.Range(
                dataProvider.GetValue<decimal?>("maxprice"),
                dataProvider.GetValue<decimal?>("minprice")
                );
            model.Filters.Brands.Reset(dataProvider.GetValues<string>("brands"));
            model.Filters.Retailers.Reset(dataProvider.GetValues<string>("retailer"));
            model.Order = CalculateSort(
                dataProvider.GetValue<string>("sort"),
                !string.IsNullOrEmpty(model.Query),
                !string.IsNullOrEmpty(model.Filters.Department));

            return model;
        }

        private Sort CalculateSort(string sort, bool hasQuery, bool hasDepartmentFilter)
        {
            switch (sort)
            {
                case "priceasc":
                    return Sort.PriceAsc;
                case "pricedesc":
                    return Sort.PriceDesc;
                default:
                    if (!hasQuery)
                    {
                        return hasDepartmentFilter ? Sort.PriorityThenRandom : Sort.RandomOrder;
                    }
                    return Sort.Relevance;
            }
        }
    }
}