using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopomo.Searchers
{
    //This should be a domain model nothing fancy.
    public class ProductSearchResults
    {
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
}
