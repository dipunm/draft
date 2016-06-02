using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Shopomo.Searchers;
using Shopomo.Searchers.QueryModels;
using Shopomo.Web.Models;

namespace Shopomo.Web.Controllers.Api
{
    public class ProductSearchController : ApiController
    {
        private readonly IProductSearcher _productSearcher;

        public ProductSearchController(IProductSearcher productSearcher)
        {
            _productSearcher = productSearcher;
        }

        public async Task<IHttpActionResult> FullSearch(PagedSearchModel userSearch)
        {
            var query = ProductSearch.Build(userSearch)
                .IncludingRelatedDepartments()
                .IncludingRelatedFilters("brands")
                .IncludingRelatedFilters("retailers");

            var results = await _productSearcher.SearchAsync(query);

            var viewModel = new SearchResults(results, userSearch);
            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> GetProductDetails(string productId)
        {
            var query = ProductSearch.BuildSearchFromId(productId);
            var results = await _productSearcher.SearchAsync(query);
            var details = results.GetProducts().FirstOrDefault();

            if (details == null)
                return NotFound();

            return Ok(details);
        }
    }
}