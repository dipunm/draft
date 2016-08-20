using System;
using System.Collections.Generic;
using System.Threading;

namespace Shopomo.ProductSearcher.Domain.Search
{
    public class SearchModel
    {
        public SearchModel()
        {
            Filters = new SearchFilters();
            Page = new PageModel();
        }
        public string Query { get; set; }
        public SearchFilters Filters { get; }
        public Sort Order { get; set; }
        public PageModel Page { get; }
    }

    public class PageModel
    {
        private const int MaxPageSize = 200;

        internal PageModel()
        {
            Start = 0;
            Size = 10;
        }
        public int Start { get; set;  }
        public int Size { get; set; }

        public void Change(int start, int size)
        {
            if (size > MaxPageSize)
                throw new ArgumentException($"A page size over {MaxPageSize} is not allowed.", nameof(size));
            if (start < 0)
                throw new ArgumentException("A negative number is not allowed", nameof(start));
            if (size < 0)
                throw new ArgumentException("A negative number is not allowed", nameof(size));

            Start = start;
            Size = size;
        }
    }

    public class SearchFilters
    {
        public PriceRange PriceRange { get; set; }
        public string Department { get; set; }
        public ICollection<string> Brands { get; } = new List<string>();
        public ICollection<string> Retailers { get; } = new List<string>();
        public SaleOption Sale { get; set; }
        public bool? WithFreeDelivery { get; set; }
    }

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

    public class SaleOption
    {
        private SaleOption(int value, string displayText)
        {
            _value = value;
            DisplayText = displayText;
        }

        private readonly decimal _value;
        public string DisplayText { get; }

        public static implicit operator decimal? (SaleOption opt)
        {
            return opt?._value;
        }

        public static implicit operator SaleOption(string alias)
        {
            switch (alias)
            {
                case "all":
                    return new SaleOption(0, "All on sale");
                case "10off":
                    return new SaleOption(10, "10% or more");
                case "25off":
                    return new SaleOption(25, "25% or more");
                case "50off":
                    return new SaleOption(50, "50% or more");
                default:
                    return null;
            }
        }
    }

    public enum Sort
    {
        Relevance = 0, PriceAsc, PriceDesc,
        RandomOrder, PriorityThenRandom
    }
}