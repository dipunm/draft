using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopomo.Web.Models.Binders
{
    public class SearchModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(SearchModel))
            {
                var valueProvider = bindingContext.ValueProvider;
                return new SearchModel()
                {
                    QueryText = valueProvider.GetValue("q")?.AttemptedValue,
                    SortBy = GetSortValue(valueProvider.GetValue("sort"))
                };
            }

            return base.BindModel(controllerContext, bindingContext);
        }

        private static Sort GetSortValue(ValueProviderResult value)
        {
            switch (value?.AttemptedValue)
            {
                case "priceasc":
                    return Sort.PriceAsc;
                case "pricedesc":
                    return Sort.PriceDesc;
                default:
                    return Sort.Relevance;
            }
        }
    }
}