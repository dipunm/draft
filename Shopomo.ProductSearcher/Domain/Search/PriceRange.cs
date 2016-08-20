namespace Shopomo.ProductSearcher.Domain.Search
{
    public class PriceRange
    {
        private PriceRange(decimal? min, decimal? max)
        {
            Min = min;
            Max = max;
        }

        public decimal? Min { get; }
        public decimal? Max { get; }

        public static PriceRange Range(decimal? min, decimal? max)
        {
            if (min < 0 || max < 0)
                return null;
            if (min == null && max == null)
                return null;

            return new PriceRange(min, max);
        }
    }
}