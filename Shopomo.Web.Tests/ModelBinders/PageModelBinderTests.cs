using Moq;
using NUnit.Framework;
using ReturnNull.ValueProviders;
using Shopomo.Web.Models.Binders;
using Shouldly;

namespace Shopomo.Web.Tests.ModelBinders
{
    [TestFixture]
    public class SearchModelBuilderTests
    {
        [Test]
        public void SearchModelBuilder_ShouldReadPropertiesFromQuerystringOnly()
        {
            var valueProvider = new Mock<IValueProvider>();
            var querystringValueProvider = new Mock<IValueProvider>();
            valueProvider.Setup(p => p.LimitedTo("querystring"))
                .Returns(querystringValueProvider.Object);
            var builder = new SearchModelBuilder();

            builder.BuildModel(valueProvider.Object);

            querystringValueProvider.Verify(p => p.GetValue("q", default(string)));
        }

        [Test]
        public void PageModelBuilder_ShouldReadPropertiesFromQuerystringOnly()
        {
            var valueProvider = new Mock<IValueProvider>();
            var querystringValueProvider = new Mock<IValueProvider>();
            valueProvider.Setup(p => p.LimitedTo("querystring"))
                .Returns(querystringValueProvider.Object);
            var builder = new PageModelBuilder();

            builder.BuildModel(valueProvider.Object);

            querystringValueProvider.Verify(p => p.GetValue("pagesize", It.IsAny<int>()));
        }

        [Test]
        public void PageModelBuilder_WhenPageSizeIsGreaterThan100_ShouldLimitPageSizeTo100()
        {
            var valueProvider = new Mock<IValueProvider>();
            var querystringValueProvider = new Mock<IValueProvider>();
            valueProvider.Setup(p => p.LimitedTo("querystring"))
                .Returns(querystringValueProvider.Object);
            querystringValueProvider.Setup(p => p.GetValue("pagesize", It.IsAny<int>()))
                .Returns(101);
            var builder = new PageModelBuilder();

            var model = builder.BuildModel(valueProvider.Object);

            model.Size.ShouldBe(100);
        }

        [Test]
        public void PageModelBuilder_ShouldHaveDefaultPageSizeOf9()
        {
            var valueProvider = new Mock<IValueProvider>();
            var querystringValueProvider = new Mock<IValueProvider>();
            valueProvider.Setup(p => p.LimitedTo("querystring"))
                .Returns(querystringValueProvider.Object);
            var builder = new PageModelBuilder();

            var model = builder.BuildModel(valueProvider.Object);

            querystringValueProvider.Verify(p => p.GetValue("pagesize", 9));
        }

        [Test]
        public void PageModelBuilder_ShouldHandleNegativeNumbersMakingThemPositive()
        {
            var valueProvider = new Mock<IValueProvider>();
            var querystringValueProvider = new Mock<IValueProvider>();
            valueProvider.Setup(p => p.LimitedTo("querystring"))
                .Returns(querystringValueProvider.Object);
            var builder = new PageModelBuilder();
            querystringValueProvider.Setup(p => p.GetValue("pagesize", It.IsAny<int>()))
                .Returns(-10);

            var model = builder.BuildModel(valueProvider.Object);

            model.Size.ShouldBe(10);
        }
    }
}
