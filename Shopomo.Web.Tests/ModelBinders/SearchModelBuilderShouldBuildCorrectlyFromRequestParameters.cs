using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ReturnNull.ValueProviders;
using Shopomo.ProductSearcher;
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
            _datasources = new Dictionary<string, IValueSource>()
            {
                { "querystring", _datasource.Object },
            };
            _valueProvider = new ValueProvider(_datasources);
            _builder = new SearchModelBuilder();
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
        public void SearchModelBuilder_WhenSortProvided_ShouldSetModelOrderToProvidedValue()
        {
            _datasource.Setup(s => s.GetValues<Sort>("sort"))
                .Returns(new[] { Sort.PriceAsc });

            var model = _builder.BuildModel(_valueProvider);

            model.Order.ShouldBe(Sort.PriceAsc);
        }


        [Test]
        public void SearchModelBuilder_WhenSortNotAvailable_ShouldSetSortToRelevanceOrder()
        {
            _datasource.Setup(s => s.GetValues<Sort>("sort"))
                .Returns(new Sort[0]);

            var model = _builder.BuildModel(_valueProvider);

            model.Order.ShouldBe(Sort.Relevance);
        }

        [Test]
        public void SearchModelBuilder_WhenPageSizeIsGreaterThan100_ShouldLimitPageSizeTo100()
        {
            _datasource.Setup(s => s.GetValues<int>("pagesize"))
                .Returns(new[] {101});

            var model = _builder.BuildModel(_valueProvider);

            model.Page.Size.ShouldBe(100);
        }

        [Test]
        public void SearchModelBuilder_ShouldHaveDefaultPageSizeOf9()
        {
            _datasource.Setup(s => s.GetValues<int>("pagesize"))
                .Returns(new int[0]);

            var model = _builder.BuildModel(_valueProvider);

            model.Page.Size.ShouldBe(9);
        }

        [Test]
        public void SearchModelBuilder_ShouldHandleNegativePageStartMakingItPositive()
        {
            _datasource.Setup(s => s.GetValues<int>("pagestart"))
                .Returns(new [] {-10});

            var model = _builder.BuildModel(_valueProvider);

            model.Page.Start.ShouldBe(10);
        }

        [Test]
        public void SearchModelBuilder_ShouldHandleNegativePageSizeMakingItPositive()
        {
            _datasource.Setup(s => s.GetValues<int>("pagesize"))
                .Returns(new[] { -10 });

            var model = _builder.BuildModel(_valueProvider);

            model.Page.Size.ShouldBe(10);
        }

        [Test]
        public void SearchModelBuilder_ShouldGetPagePosition_UsingCorrectKey()
        {
            _datasource.Setup(s => s.GetValues<int>("pagestart"))
                .Returns(new[] { 10 });

            var model = _builder.BuildModel(_valueProvider);

            model.Page.Start.ShouldBe(10);
        }
    }
}
