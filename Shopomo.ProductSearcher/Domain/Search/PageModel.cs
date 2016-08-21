using System;

namespace Shopomo.ProductSearcher.Domain.Search
{
    public class PageModel
    {
        private const int MaxPageSize = 200;

        internal PageModel()
        {
            Start = 0;
            Size = 10;
        }

        public int Start { get; set; }
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
}