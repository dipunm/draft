using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Shopomo.Web.Models;
using Shouldly;

namespace Shopomo.Web.Tests.Controllers
{

    public class SearchListingController : Controller
    {
        private readonly IProductSearcher _searcher;

        public SearchListingController(IProductSearcher searcher)
        {
            _searcher = searcher;
        }

        public async Task<ActionResult> SearchAsync(SearchModel search)
        {
            var support = new ISupportingRequest<object>[]
            {
                new BrandOptions(5),
                new DepartmentOptions(10),
                new RetailerOptions(5),
                new SpellingSuggestion()
            };
            var result = await _searcher.SearchAsync(search, support);

            return View(model: new SearchResults()
            {
                Products = result.Products,
                BrandFilters = result.Get<BrandOptions, string[]>(),
                DepartmentFilters = result.Get<DepartmentOptions, string[]>(),
                RetailerFilters = result.Get<RetailerOptions, string[]>(),
                DidYouMean = result.Get<SpellingSuggestion, string>()
            });
        }
    }

    public interface IProductSearcher
    {
        Task<ISearchResult> SearchAsync(SearchModel query, IEnumerable<ISupportingRequest<object>> supportingRequests);
    }

    public interface ISearchResult
    {
        IEnumerable<ProductSummary> Products { get; }

        TOut Get<TRequest, TOut>() where TRequest : ISupportingRequest<TOut>;
    }

    public interface ISupportingRequest<out TOut>
    {

    }

    public abstract class FilterOptions
    {
        protected FilterOptions(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; }
    }

    public class DepartmentOptions : FilterOptions, ISupportingRequest<string[]>
    {
        public DepartmentOptions(int limit) : base(limit)
        {
        }
    }

    public class RetailerOptions : FilterOptions, ISupportingRequest<string[]>
    {
        public RetailerOptions(int limit) : base(limit)
        {
        }
    }

    public class BrandOptions : FilterOptions, ISupportingRequest<string[]>
    {
        public BrandOptions(int limit) : base(limit)
        {
        }
    }

    public class SpellingSuggestion : ISupportingRequest<string>
    {

    }

    public class ProductSummary
    {
        public string Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string ImageUrl { get; }
        public bool OnSale { get; }
        public bool FreeDelivery { get; }
        public int Rating { get; }
        public decimal OldPrice { get; }
    }

    [TestFixture]
    public class SearchListingControllerTests
    {
        private Mock<IProductSearcher> _searcher;
        private SearchListingController _controller;
        private Mock<ISearchResult> _searchResults;

        [SetUp]
        public void Setup()
        {
            _searcher = new Mock<IProductSearcher>();
            _searchResults = new Mock<ISearchResult>();
            _searcher.SetReturnsDefault(Task.FromResult(_searchResults.Object));
            _controller = new SearchListingController(_searcher.Object);
        }

        [Test]
        public async Task Search_GivenSearchModel_ShouldSearchForProductsWithModel()
        {
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(query, It.IsAny<IEnumerable<ISupportingRequest<object>>>()));
        }

        [Test]
        public async Task Search_ShouldAskFor5RetailerOptions()
        {
            const int Limit = 5;
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest<object>>>(info =>
                //it is an array (info) where:
                info.OfType<RetailerOptions>()
                .SingleOrDefault(o => o.Limit == Limit) != null)));
        }

        [Test]
        public async Task Search_ShouldAskFor5BrandOptions()
        {
            const int Limit = 5;
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest<object>>>(info =>
                //it is an array (info) where:
                info.OfType<BrandOptions>()
                .SingleOrDefault(o => o.Limit == Limit) != null)));
        }

        [Test]
        public async Task Search_ShouldAskFor10DepartmentOptions()
        {
            const int Limit = 10;
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest<object>>>(info =>
                //it is an array (info) where:
                info.OfType<DepartmentOptions>()
                .SingleOrDefault(o => o.Limit == Limit) != null)));
        }

        [Test]
        public async Task Search_ShouldAskForSpellingSuggestions()
        {
            var query = new SearchModel();

            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest<object>>>(info =>
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
            var model = (SearchResults) ((ViewResult) result).Model;

            model.Products.ShouldBe(products);
        }

        [Test]
        public async Task Search_GivenBrandOptions_ShouldPresentOptionsInViewModel()
        {
            var brands = new [] { "Nike", "Addidas", "Lacoste" };
            _searchResults.Setup(r => r.Get<BrandOptions, string[]>())
                .Returns(brands);
            
            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchResults)((ViewResult)result).Model;

            model.BrandFilters.ShouldBe(brands);
        }
        
        [Test]
        public async Task Search_GivenRetailerOptions_ShouldPresentOptionsInViewModel()
        {
            var retailers = new[] { "Morrisons", "Tesco", "Asda" };
            _searchResults.Setup(r => r.Get<RetailerOptions, string[]>())
                .Returns(retailers);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchResults)((ViewResult)result).Model;

            model.RetailerFilters.ShouldBe(retailers);
        }

        [Test]
        public async Task Search_GivenDepartmentOptions_ShouldPresentOptionsInViewModel()
        {
            var departments = new[] { "Electronics", "Clothing", "Homewear" };
            _searchResults.Setup(r => r.Get<DepartmentOptions, string[]>())
                .Returns(departments);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchResults)((ViewResult)result).Model;

            model.DepartmentFilters.ShouldBe(departments);
        }

        [Test]
        public async Task Search_GivenSpellingSuggestionOptions_ShouldPresentSpellingInViewModel()
        {
            var spelling = "corrected spelling";
            _searchResults.Setup(r => r.Get<SpellingSuggestion, string>())
                .Returns(spelling);

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchResults)((ViewResult)result).Model;

            model.DidYouMean.ShouldBe(spelling);
        }
        

        /*
         * Should Query to Searcher
         * Should Request Filter Options
         * Should Map Response to ViewModel for view
         * 
         * _productSearcher: searches for products and returns results
         * _productRepository: gets one or more products using traditional ID based queries
         * _productQuerySuggester: provides suggestions to users to help improve or complete search queries
         * 
         * _departments: 
         *  - getId: used to create user friendly hyperlinks
         *  - getPath: used to map from hyperlink to search
         *  - getAll: used to reduce hit on solr. Used to calculate other caches.
         */


    }


}
