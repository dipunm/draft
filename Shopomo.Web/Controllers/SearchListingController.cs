using System.Threading.Tasks;
using System.Web.Mvc;
using Shopomo.ProductSearcher;
using Shopomo.Web.Models;

namespace Shopomo.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchListingController : Controller
    {
        private readonly IProductSearcher _searcher;

        public SearchListingController(IProductSearcher searcher)
        {
            _searcher = searcher;
        }


        [Route("")]
        public async Task<ActionResult> SearchAsync(SearchModel search)
        {
            var support = new IHintRequest<object>[]
            {
                new RelatedBrands(5),
                new Departments(10),
                new RelatedRetailers(5),
                new SpellingSuggestion()
            };
            var result = await _searcher.SearchAsync(search, support);

            return View("Search", model: new SearchResults()
            {
                Products = result.Products,
                BrandFilters = result.Get<RelatedBrands, string[]>(),
                DepartmentFilters = result.Get<Departments, string[]>(),
                RetailerFilters = result.Get<RelatedRetailers, string[]>(),
                DidYouMean = result.Get<SpellingSuggestion, string>(),
                Total = result.Total
            });
        }


        [Route("d/{department}")]
        public Task<ActionResult> SearchByDepartment(string department, SearchModel search)
        {
            ViewData["Context"] = "Department" + department;
            if(string.IsNullOrEmpty(search.Filters.Department))
                search.Filters.Department = department;

            return SearchAsync(search);
        }

        [Route("b/{brand}")]
        public Task<ActionResult> SearchByBrand(string brand, SearchModel search)
        {
            ViewData["Context"] = "Brand" + brand;
            search.Filters.Brands = new[] { brand };
            return SearchAsync(search);
        }

    }


}