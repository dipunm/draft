using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Shopomo.Web.Models;

namespace Shopomo.Web.Controllers.Api
{
    public class ProductSearchController : ApiController
    {
        private readonly IProductQueryBuilder _productQueryBuilder;
        private readonly IProductSearcher _productSearcher;

        public ProductSearchController(IProductQueryBuilder productQueryBuilder, IProductSearcher productSearcher)
        {
            _productQueryBuilder = productQueryBuilder;
            _productSearcher = productSearcher;
        }

        public async Task<IHttpActionResult> FullSearch(PagedSearchModel userSearch)
        {
            var query = _productQueryBuilder.Build(userSearch)
                .WithDepartments()
                .WithFilters("brands")
                .WithFilters("retailers");

            var results = await _productSearcher.SearchAsync(query);

            var viewModel = new PagedSearchResults(results, userSearch);
            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> GetProductDetails(string productId)
        {
            var query = _productQueryBuilder.BuildSearchFromId(productId);
            var results = await _productSearcher.SearchAsync(query);
            var details = results.GetProducts().FirstOrDefault();

            if (details == null)
                return NotFound();

            return Ok(details);
        }
    }
}