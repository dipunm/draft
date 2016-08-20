using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shopomo.ProductSearcher.Domain.Search;
using Shouldly;

namespace Shopomo.ProductSearcher.Tests
{
    [TestFixture]
    public class SearchModelTests
    {
        [Test]
        public void DefaultSearchModel_ShouldHavePageSizeAs10()
        {
            var model = new SearchModel();

            model.Page.Size.ShouldBe(10);
        }

        [Test]
        public void DefaultSearchModel_ShouldHavePageStartAt0()
        {
            var model = new SearchModel();

            model.Page.Start.ShouldBe(0);
        }

        [Test]
        public void DefaultSearchModel_ShouldHaveFilters_NotNull()
        {
            var model = new SearchModel();

            model.Filters.ShouldNotBeNull();
        }

        [Test]
        public void DefaultSearchModel_ShouldHaveOrderByRelevance()
        {
            var model = new SearchModel();

            model.Order.ShouldBe(Sort.Relevance);
        }

        [Test]
        public void DefaultSearchModel_ShouldHaveQueryAsNull()
        {
            var model = new SearchModel();

            model.Query.ShouldBeNull();
        }

        [Test]
        public void DefaultSearchModel_Filters_AllNonEnumerablePropertiesShouldBeNull()
        {
            var model = new SearchModel();

            model.Filters.PriceRange.ShouldBeNull();
            model.Filters.Department.ShouldBeNull();
            model.Filters.WithFreeDelivery.ShouldBeNull();
            model.Filters.Sale.ShouldBeNull();
        }

        [Test]
        public void DefaultSearchModel_Filters_AllEnumerablePropertiesShouldBeEmpty()
        {
            var model = new SearchModel();

            model.Filters.Brands.ShouldBeEmpty();
            model.Filters.Retailers.ShouldBeEmpty();
        }

        [TestCase(5, 20)]
        [TestCase(0, 10)]
        public void PageModel_GivenStartAndSizeValues_ShouldStoreValuesInStartAndSizeProperties(int start, int size)
        {
            var model = new SearchModel();
            model.Page.Change(start, size);

            model.Page.Start.ShouldBe(start);
            model.Page.Size.ShouldBe(size);
        }

        [Test]
        public void PageModel_GivenSizeOver200_ShouldThrowArgumentException()
        {
            var model = new SearchModel();
            Assert.Throws<ArgumentException>(() => model.Page.Change(0, 201));
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void PageModel_GivenNegativeValues_ShouldThrowArgumentException(int page, int size)
        {
            var model = new SearchModel();
            Assert.Throws<ArgumentException>(() => model.Page.Change(page, size));
        } 

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void PriceRange_GivenNegativePrices_ShouldReturnNull(decimal min, decimal max)
        {
            PriceRange.Range(min, max).ShouldBeNull();
        }

        [Test]
        public void PriceRange_GivenNullForMinAndMax_ShouldReturnNull()
        {
            PriceRange.Range(null, null).ShouldBeNull();
        }

        [TestCase(1, 2)]
        [TestCase(null, 2)]
        [TestCase(1, null)]
        [TestCase(25.90, 35.00)]
        public void PriceRange_GivenMinAndMaxValues_ShouldStoreValuesInMinAndMaxProperties(decimal? min, decimal? max)
        {
            var range = PriceRange.Range(min, max);
            range.Min.ShouldBe(min);
            range.Max.ShouldBe(max);
        }

        //saleoption
    }
}
