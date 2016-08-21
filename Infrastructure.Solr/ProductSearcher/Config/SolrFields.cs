namespace Infrastructure.Solr.ProductSearcher.Config
{
    public static class SolrFields
    {
        public const string DepartmentFilter = "departmentpath";
        public const string PriceFilter = "price";
        public const string RetailerFilter = "retailerid";
        public const string BrandFilter = "brandid";
        public const string DeliveryFilter = "freedelivery";
        public const string DiscountFilter = "discountpercentage";
        public const string RandomOrder = "randomorder";
        public const string PriorityOrder = "listingpriority";
        public const string PriceOrder = PriceFilter;
        public const string DepartmentFacet = DepartmentFilter;
        public const string BrandFacet = BrandFilter;
        public const string RetailerFacet = RetailerFilter;
        public const string DiscountMax = DiscountFilter;
    }
}