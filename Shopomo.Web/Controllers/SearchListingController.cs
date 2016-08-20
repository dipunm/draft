using System.Threading.Tasks;
using System.Web.Mvc;
using Library.Core;
using Shopomo.ProductSearcher;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.Search;
using Shopomo.ProductSearcher.Domain.SearchMetas;
using Shopomo.Web.Models;

namespace Shopomo.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchListingController : Controller
    {
        private readonly IProductSearcher _productSearcher;

        public SearchListingController(IProductSearcher productSearcher)
        {
            _productSearcher = productSearcher;
        }

        private ISearchMeta<object>[] PageInterests { get; } = {
            new RelatedBrands(5),
            new Departments(10),
            new RelatedRetailers(5),
            new SpellingSuggestion()
        };

        private void LimitPaginationOnSearch(SearchModel search)
        {
            search.Page.Change(0, 10);
        }

        [Route("")]
        public async Task<ActionResult> SearchAsync(SearchModel search)
        {
            LimitPaginationOnSearch(search);
            var result = await _productSearcher.SearchAsync(search, PageInterests);
            return View("Search", new SearchListingModel(result));
        }

        [Route("d/{department}")]
        public Task<ActionResult> SearchByDepartment(string department, SearchModel search)
        {
            ViewData["Context"] = "Department" + department;
            if (string.IsNullOrEmpty(search.Filters.Department))
                search.Filters.Department = department;

            return SearchAsync(search);
        }

        [Route("b/{brand}")]
        public Task<ActionResult> SearchByBrand(string brand, SearchModel search)
        {
            ViewData["Context"] = "Brand" + brand;
            search.Filters.Brands.Reset(new []{ brand });
            return SearchAsync(search);
        }

    }


}