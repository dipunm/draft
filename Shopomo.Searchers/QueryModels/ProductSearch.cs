namespace Shopomo.Searchers.QueryModels
{
    //todo: simplify
    public class ProductSearch
    {
        public static ProductSearch Build(object userSearch) { return new ProductSearch(); }
        public static ProductSearch BuildFromText(string text) { return new ProductSearch(); }
        public static ProductSearch BuildSearchFromId(string productId) { return new ProductSearch(); }

        public ProductSearch WithoutProducts()
        {
            return this;
        }
        public ProductSearch IncludingRelatedFilters(string filterType, int limit = 5)
        {
            return this;
        }
        public ProductSearch IncludingRelatedDepartments()
        {
            return this;
        }

        public ProductSearch IncludingSpellingSuggestion()
        {
            return this;
        }
    }
}