﻿namespace Shopomo.ProductSearcher.Domain.AdditionalData
{
    public abstract class FilterOptions
    {
        protected FilterOptions(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; }
    }
}