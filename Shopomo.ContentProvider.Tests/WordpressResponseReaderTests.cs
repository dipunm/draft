using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Shopomo.ContentProvider.Wordpress;
using Shouldly;

namespace Shopomo.ContentProvider.Tests
{
    [TestFixture]
    public class WordpressResponseReaderShouldConvertResponseToObject
    {
        [Test]
        public void GivenNullContent_ShouldReturnNull()
        {
            var reader = new WordpressResponseReader();
            var model = reader.GetContent(null);
            model.ShouldBe(null);
        }

        [Test]
        public void GivenEmptyContent_ShouldReturnNull()
        {
            var reader = new WordpressResponseReader();
            var model = reader.GetContent("");
            model.ShouldBe(null);
        }

        [Test]
        public void GivenContentInJson_ShouldGetPageTitle()
        {
            var reader = new WordpressResponseReader();
            var model = reader.GetContent("{page: {title: '', content: '<p>HTML</p>'}}");
            model.Content.ShouldBe("<p>HTML</p>");
        }

        [Test]
        public void GivenTitleInJson_ShouldGetPageContent()
        {
            var reader = new WordpressResponseReader();
            var model = reader.GetContent("{page: {title: 'Page Title', content: ''}}");
            model.Title.ShouldBe("Page Title");
        }
    }
}
