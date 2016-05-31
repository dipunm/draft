using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopomo.Web.Controllers.Api
{

    public interface IDepartmentSearcher
    {
        Task<IEnumerable<object>> GetAllDepartmentsAsync();
    }

    public interface IProductSearcher
    {
        Task<ProductSearchResults> SearchAsync(ProductQuery productQuery);
    }

    public class ProductSearchResults
    {
        public IEnumerable<object> GetRelatedFilters(string filterType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetProducts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetDepartments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetFilters(string filterType)
        {
            throw new NotImplementedException();
        }

        public string GetSpellingSuggestion()
        {
            throw new NotImplementedException();
        }
    }

    public class ProductQuery
    {
        public ProductQuery WithoutProducts()
        {
            return this;
        }
        public ProductQuery WithFilters(string filterType, int limit = 5)
        {
            return this;
        }
        public ProductQuery WithDepartments()
        {
            return this;
        }

        public ProductQuery WithSpellingSuggestion()
        {
            return this;
        }
    }
    public interface IProductQueryBuilder
    {
        ProductQuery Build(object userSearch);
        ProductQuery BuildFromText(string text);
        ProductQuery BuildSearchFromId(string productId);
    }
}