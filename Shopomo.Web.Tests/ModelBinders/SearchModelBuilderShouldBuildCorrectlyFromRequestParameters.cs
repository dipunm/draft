using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ReturnNull.ValueProviders;
using Shopomo.ProductSearcher.Domain.Search;
using Shopomo.Web.Models.Binders;
using Shouldly;

namespace Shopomo.Web.Tests.ModelBinders
{
    [TestFixture]
    public class SearchModelBuilderShouldBuildCorrectlyFromRequestParameters
    {
        private Dictionary<string, IValueSource> _datasources;
        private ValueProvider _valueProvider;
        private SearchModelBuilder _builder;
        private Mock<IValueSource> _datasource;

        [SetUp]
        public void Setup()
        {
            _datasource = new Mock<IValueSource>();
            _datasources = new Dictionary<string, IValueSource>
            {
                {"querystring", _datasource.Object}
            };
            _valueProvider = new ValueProvider(_datasources);
            _builder = new SearchModelBuilder();
        }

        [TestCase("priceasc", Sort.PriceAsc)]
        [TestCase("pricedesc", Sort.PriceDesc)]
        public void SearchModelBuilder_WhenSortProvided_ShouldSetModelOrderToProvidedValue(string sourceValue,
            Sort destValue)
        {
            _datasource.Setup(s => s.GetValues<string>("sort"))
                .Returns(new[] {sourceValue});

            var model = _builder.BuildModel(_valueProvider);

            model.Order.ShouldBe(destValue);
        }

        [Test]
        public void SearchModelBuilder_ShouldNotChangeDefaultPagesize()
        {
            var defaultModel = new SearchModel();

            var model = _builder.BuildModel(_valueProvider);

            model.Page.Start.ShouldBe(defaultModel.Page.Start);
            model.Page.Size.ShouldBe(defaultModel.Page.Size);
        }

        [Test]
        public void SearchModelBuilder_ShouldReadPropertiesFromQuerystringOnly()
        {
            var datasource2 = new Mock<IValueSource>();
            _datasources.Add("notquerystring", datasource2.Object);

            _builder.BuildModel(_valueProvider);

            datasource2.Verify(p => p.GetValues<string>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<int>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<int?>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<double>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<double?>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<decimal>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<decimal?>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<float>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<float?>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<bool>(It.IsAny<string>()), Times.Never);
            datasource2.Verify(p => p.GetValues<bool?>(It.IsAny<string>()), Times.Never);
        }


        [Test]
        public void SearchModelBuilder_WhenSortNotAvailable_GivenASearchTerm_ShouldSetSortToRelevanceOrder()
        {
            _datasource.Setup(s => s.GetValues<string>("q"))
                .Returns(new[] {"my custom search"});
            _datasource.Setup(s => s.GetValues<Sort>("sort"))
                .Returns(new Sort[0]);

            var model = _builder.BuildModel(_valueProvider);

            model.Order.ShouldBe(Sort.Relevance);
        }

        [Test]
        public void SearchModelBuilder_WhenSortNotAvailableAndNoSearchProvided_ShouldSetSortToRandomOrder()
        {
            _datasource.Setup(s => s.GetValues<string>("q"))
                .Returns(new string[0]);
            _datasource.Setup(s => s.GetValues<string>("sort"))
                .Returns(new string[0]);

            var model = _builder.BuildModel(_valueProvider);

            model.Order.ShouldBe(Sort.RandomOrder);
        }

        [Test]
        public void
            SearchModelBuilder_WhenSortNotAvailableAndNoSearchProvidedButDepartmentProvided_ShouldSetSortToRandomOrderWithListingPriority
            ()
        {
            _datasource.Setup(s => s.GetValues<string>("q"))
                .Returns(new string[0]);
            _datasource.Setup(s => s.GetValues<string>("sort"))
                .Returns(new string[0]);
            _datasource.Setup(s => s.GetValues<string>("department"))
                .Returns(new[] {"somedepartment"});

            var model = _builder.BuildModel(_valueProvider);

            model.Order.ShouldBe(Sort.PriorityThenRandom);
        }
    }
}