using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Shopomo.Web.Controllers;
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
            var support = new ISupportingRequest[]
            {
                new BrandOptions(5),
                new DepartmentOptions(10),
                new RetailerOptions(5),
                new SpellingSuggestion()
            };
            var result = await _searcher.SearchAsync(search, support);

            return View(new SearchResults());
        }
    }

    public interface IProductSearcher
    {
        Task<object> SearchAsync(SearchModel query, IEnumerable<ISupportingRequest> supportingRequests);
    }


    public interface ISupportingRequest
    {

    }

    public abstract class FilterOptions : ISupportingRequest
    {
        protected FilterOptions(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; }
    }

    public class DepartmentOptions : FilterOptions
    {
        public DepartmentOptions(int limit) : base(limit)
        {
        }
    }

    public class RetailerOptions : FilterOptions
    {
        public RetailerOptions(int limit) : base(limit)
        {
        }
    }

    public class BrandOptions : FilterOptions
    {
        public BrandOptions(int limit) : base(limit)
        {
        }
    }

    public class SpellingSuggestion : ISupportingRequest
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

        [SetUp]
        public void Setup()
        {
            _searcher = new Mock<IProductSearcher>();
            _controller = new SearchListingController(_searcher.Object);
        }

        [Test]
        public async Task Search_GivenSearchModel_ShouldSearchForProductsWithModel()
        {
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(query, It.IsAny<IEnumerable<ISupportingRequest>>()));
        }

        [Test]
        public async Task Search_ShouldAskFor5RetailerOptions()
        {
            const int Limit = 5;
            var query = new SearchModel();
            
            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest>>(info =>
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

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest>>(info =>
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

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest>>(info =>
                //it is an array (info) where:
                info.OfType<DepartmentOptions>()
                .SingleOrDefault(o => o.Limit == Limit) != null)));
        }

        [Test]
        public async Task Search_ShouldAskForSpellingSuggestions()
        {
            var query = new SearchModel();

            await _controller.SearchAsync(query);

            _searcher.Verify(s => s.SearchAsync(It.IsAny<SearchModel>(), It.Is<IEnumerable<ISupportingRequest>>(info =>
                //it is an array (info) where:
                info.OfType<SpellingSuggestion>()
                .SingleOrDefault() != null)));
        }


        /*
         * ABOVE: Request
         * BELOW: Response
         * 
         * Given product results, should contain product information in view model
         * Given SpellingSuggestion, Should contain suggestion in view model
         * Given RetailerOptions, Should contain options in view model
         * Given BrandOptions, Should contain options in view model
         * Given DepartmentOptions, Should contain options in view model
         * 
         * 
         */

        public async Task Search_GivenProductResults_ShouldPrensentProductInformationInViewModel()
        {
            var products = new List<ProductSummary>() {new ProductSummary(), new ProductSummary(), new ProductSummary()};
            _searcher.Setup(s => s.SearchAsync(
                It.IsAny<SearchModel>(), 
                It.IsAny<IEnumerable<ISupportingRequest>>()))
                .ReturnsAsync(new
                {
                    Products = products
                });

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchResults) ((ViewResult) result).Model;

            model.Products.ShouldBe(products);
        }

        public async Task Search_GivenBrandOptions_ShouldPresentOptionsInViewModel()
        {
            var brands = new List<string>() { "Nike", "Addidas", "Lacoste" };
            _searcher.Setup(s => s.SearchAsync(
                It.IsAny<SearchModel>(),
                It.IsAny<IEnumerable<ISupportingRequest>>()))
                .ReturnsAsync(new
                {
                    SupportingData = new[] {brands}
                });

            var result = await _controller.SearchAsync(new SearchModel());
            var model = (SearchResults)((ViewResult)result).Model;

//            model.Brands.ShouldBe(brands);
        }
        public async Task Search_GivenRetailerOptions_ShouldPresentOptionsInViewModel()
        {

        }
        public async Task Search_GivenDepartmentOptions_ShouldPresentOptionsInViewModel()
        {

        }
        public async Task Search_GivenSpellingSuggestionOptions_ShouldPresentSpellingInViewModel()
        {

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
