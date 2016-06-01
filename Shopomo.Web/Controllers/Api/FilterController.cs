using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Shopomo.Web.Models;

namespace Shopomo.Web.Controllers.Api
{
    public class FilterController : ApiController
    {
        private readonly IProductQueryBuilder _productQueryBuilder;
        private readonly IProductSearcher _productSearcher;
        private readonly IDepartmentSearcher _departmentSearcher;

        public FilterController(IProductQueryBuilder productQueryBuilder, IProductSearcher productSearcher, IDepartmentSearcher departmentSearcher)
        {
            _productQueryBuilder = productQueryBuilder;
            _productSearcher = productSearcher;
            _departmentSearcher = departmentSearcher;
        }

        public async Task<IHttpActionResult> GetFilters(string filterType, PagedSearchModel userSearch)
        {
            var query = _productQueryBuilder.Build(userSearch)
                .WithoutProducts()
                .WithFilters(filterType, 100);

            var results = await _productSearcher.SearchAsync(query);

            var filters = results.GetRelatedFilters(filterType);
            return Ok(filters);
        }

        public async Task<IHttpActionResult> GetRelevantDepartmentsUnder(string departmentId, PagedSearchModel userSearch)
        {
            userSearch.DepartmentId = departmentId ?? userSearch.DepartmentId;
            var query = _productQueryBuilder.Build(userSearch)
                .WithoutProducts()
                .WithDepartments();

            var results = await _productSearcher.SearchAsync(query);

            var filters = results.GetDepartments();
            return Ok(filters);
        }

        public async Task<IHttpActionResult> GetNestedDepartments(string parentDepartmentId)
        {
            var departments = await _departmentSearcher.GetAllDepartmentsAsync();
            var nested = departments.Nest();
            return Ok(nested);
        }
    }

    public static class DepartmentsCompositionHelper
    {
        public static IEnumerable<object> Nest(this IEnumerable<object> departments)
        {
            throw new NotImplementedException();
        } 

        public static IEnumerable<object> SimplifyForListing(this IEnumerable<object> products)
        {
            throw new NotImplementedException();
        }
    }

}