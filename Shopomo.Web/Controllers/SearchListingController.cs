using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shopomo.Searchers;
using Shopomo.Searchers.QueryModels;
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

        public SearchListingController()
        {
            //empty controller for manual testing.
        }

        [Route("")]
        public ActionResult Test(SearchModel model, PageModel page)
        {
            return Json(new
            {
                Search = model,
                Page = page
            }, JsonRequestBehavior.AllowGet);
        }

        /*
        [Route("")]
        public async Task<ActionResult> Search(SearchModel model)
        {
            ProductSearchResults results;
            try
            {
                var query = ProductSearch.Build(model)
                    .IncludingRelatedDepartments()
                    .IncludingRelatedFilters("brands")
                    .IncludingRelatedFilters("retailers")
                    .IncludingSpellingSuggestion();

                results = await _productSearcher.SearchAsync(query);
            }
            catch (Exception e)
            {
                results = null;
            }
            var viewModel = new SearchResults(results, model);

            if (model.QueryText?.Contains("z") ?? false)
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
        }*/
    }
}