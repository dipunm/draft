using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.Projections;
using Shopomo.ProductSearcher.Domain.SearchMetas;
using Shopomo.Web.Controllers;
using Shopomo.Web.Models;
using Shouldly;

namespace Shopomo.Web.Tests.Controllers
{

    [TestFixture]
    public class SearchListingControllerRetrievesAllRequiredDataForSearchListingView
    {
        private Mock<IProductSearcher> _searcher;
        private SearchListingController _controller;
        private Mock<ISearchResult<ProductSummary>> _searchResults;

        [SetUp]
        public void Setup()
        {
            _searcher = new Mock<IProductSearcher>();
            _searchResults = new Mock<ISearchResult<ProductSummary>>();
            _searcher.SetReturnsDefault(Task.FromResult(_searchResults.Object));
            _controller = new SearchListingController(_searcher.Object);
        }

        [Test]
        public async Task Search_GivenSearchModel_ShouldSearchForProductsWithModel()
        {
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(query, It.IsAny<IEnumerable<ISearchMeta<object>>>()));
        }

        [Test]
        public async Task Search_GivenSearchModelPageStart_ShouldResetPageStartTo0()
        {
            var query = new SearchModel()
            { Page = new PageModel() {Start = 2} };

            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.Is<SearchModel>(q => 
                q.Page.Start == 0), 
                It.IsAny<IEnumerable<ISearchMeta<object>>>()));
        }

        [Test]
        public async Task Search_GivenSearchModelPageSize_ShouldResetPageSizeTo10()
        {
            var query = new SearchModel()
            {Page = new PageModel() {Size = 20} };

            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.Is<SearchModel>(q =>
                q.Page.Size == 10),
                It.IsAny<IEnumerable<ISearchMeta<object>>>()));

        }

        [Test]
        public async Task Search_ShouldAskFor5RetailerOptions()
        {
            const int Limit = 5;
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISearchMeta<object>>>(info =>
                //it is an array (info) where:
                info.OfType<RelatedRetailers>()
                .SingleOrDefault(o => o.Limit == Limit) != null)));
        }

        [Test]
        public async Task Search_ShouldAskFor5BrandOptions()
        {
            const int Limit = 5;
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISearchMeta<object>>>(info =>
                //it is an array (info) where:
                info.OfType<RelatedBrands>()
                .SingleOrDefault(o => o.Limit == Limit) != null)));
        }

        [Test]
        public async Task Search_ShouldAskFor10DepartmentOptions()
        {
            const int Limit = 10;
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISearchMeta<object>>>(info =>
                //it is an array (info) where:
                info.OfType<Departments>()
                .SingleOrDefault(o => o.Limit == Limit) != null)));
        }

        [Test]
        public async Task Search_ShouldAskForSpellingSuggestions()
        {
            var query = new SearchModel();

            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISearchMeta<object>>>(info =>
                //it is an array (info) where:
                info.OfType<SpellingSuggestion>()
                .SingleOrDefault() != null)));
        }

        [Test]
        public async Task Search_GivenProductResults_ShouldPrensentProductInformationInViewModel()
        {
            var products = new List<ProductSummary>() {new ProductSummary(), new ProductSummary(), new ProductSummary()};
            _searchResults.Setup(r => r.Products)
                .Returns(products);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchListingModel) ((ViewResult) result).Model;

            model.Products.ShouldBe(products);
        }

        [Test]
        public async Task Search_GivenBrandOptions_ShouldPresentOptionsInViewModel()
        {
            var brands = new [] { "Nike", "Addidas", "Lacoste" };
            _searchResults.Setup(r => r.Get<RelatedBrands, string[]>())
                .Returns(brands);
            
            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchListingModel)((ViewResult)result).Model;

            model.BrandFilters.ShouldBe(brands.Select(AsDisplayableValue));
        }

        [Test]
        public async Task Search_GivenRetailerOptions_ShouldPresentOptionsInViewModel()
        {
            var retailers = new[] { "Morrisons", "Tesco", "Asda" };
            _searchResults.Setup(r => r.Get<RelatedRetailers, string[]>())
                .Returns(retailers);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchListingModel)((ViewResult)result).Model;

            model.RetailerFilters.ShouldBe(retailers.Select(AsDisplayableValue));
        }

        [Test]
        public async Task Search_GivenDepartmentOptions_ShouldPresentOptionsInViewModel()
        {
            var departments = new[] { "Electronics", "Clothing", "Homewear" };
            _searchResults.Setup(r => r.Get<Departments, string[]>())
                .Returns(departments);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchListingModel)((ViewResult)result).Model;

            model.DepartmentFilters.ShouldBe(departments.Select(AsDisplayableValue));
        }

        [Test]
        public async Task Search_GivenSpellingSuggestionOptions_ShouldPresentSpellingInViewModel()
        {
            var spelling = "corrected spelling";
            _searchResults.Setup(r => r.Get<SpellingSuggestion, string>())
                .Returns(spelling);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchListingModel)((ViewResult)result).Model;

            model.DidYouMean.ShouldBe(spelling);
        }

        [Test]
        public async Task Search_GivenATotal_ShouldPresentTotalInViewModel()
        {
            var total = 1234;
            _searchResults.Setup(r => r.Total)
                .Returns(total);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchListingModel)((ViewResult)result).Model;

            model.Total.ShouldBe(total);
        }


        private DisplayableValue AsDisplayableValue(string val)
        {
            return new DisplayableValue(val);
        }
    }


}
