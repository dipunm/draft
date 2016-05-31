using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Shopomo.Web.Controllers.Api;
using Shopomo.Web.Models;

namespace Shopomo.Web.Controllers
{
    public class SearchListingController : Controller
    {
        private readonly IProductQueryBuilder _productQueryBuilder;
        private readonly IProductSearcher _productSearcher;

        public SearchListingController(IProductQueryBuilder productQueryBuilder, IProductSearcher productSearcher)
        {
            _productQueryBuilder = productQueryBuilder;
            _productSearcher = productSearcher;
        }

        public async Task<ActionResult> Search([FromUri]UriSearchModel search)
        {
            var query = _productQueryBuilder.Build(search)
                .WithDepartments()
                .WithFilters("brands")
                .WithFilters("retailers")
                .WithSpellingSuggestion();

            var results = await _productSearcher.SearchAsync(query);
            var viewModel = new FirstSearchResult(results);

            return View(viewModel);
        }

        public Task<ActionResult> SearchByDepartment(string departmentId, [FromUri] UriSearchModel search)
        {
            search.DepartmentId = departmentId;
            return Search(search);
        }

        public Task<ActionResult> SearchByBrand(string brandId, [FromUri] UriSearchModel search)
        {
            search.BrandIds = new[] {brandId};
            return Search(search);
        }
    }
}