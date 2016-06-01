using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shopomo.Web.Controllers.Api;
using Shopomo.Web.Models;

namespace Shopomo.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchListingController : Controller
    {
        private readonly IProductQueryBuilder _productQueryBuilder;
        private readonly IProductSearcher _productSearcher;

        public SearchListingController(IProductQueryBuilder productQueryBuilder, IProductSearcher productSearcher)
        {
            _productQueryBuilder = productQueryBuilder;
            _productSearcher = productSearcher;
        }

        public SearchListingController()
        {
            
        }

        [Route("")]
        public async Task<ActionResult> Search(SearchModel search)
        {
            ProductSearchResults results;
            try
            {
                var query = _productQueryBuilder.Build(search)
                    .WithDepartments()
                    .WithFilters("brands")
                    .WithFilters("retailers")
                    .WithSpellingSuggestion();

                results = await _productSearcher.SearchAsync(query);
            }
            catch (Exception e)
            {
                results = null;
            }
            var viewModel = new FirstSearchResult(results, search);
            if (search.QueryText.Contains("z"))
                viewModel.DidYouMean = "money money money";
            return View("Search", viewModel);
        }

        [Route("d/{id}/{slug}")]
        public Task<ActionResult> SearchByDepartment(string id, SearchModel search)
        {
            ViewData["Context"] = "Department" + id;
            search.DepartmentId = id;
            return Search(search);
        }

        [Route("b/{id}/{slug}")]
        public Task<ActionResult> SearchByBrand(string id, SearchModel search)
        {
            ViewData["Context"] = "Brand" + id;
            search.BrandIds = new[] { id };
            return Search(search);
        }
    }
}