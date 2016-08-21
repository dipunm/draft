using System;
using NUnit.Framework;
using Shopomo.ContentProvider.Wordpress;
using Shouldly;

namespace Shopomo.ContentProvider.Tests
{
    /// <summary>
    /// Ensures that WordpressSettings is self validating
    /// </summary>
    [TestFixture]
    [Description("Verifies that Wordpress settings are self validating and fail-fast.")]
    public class WordpressSettingsShouldBeFailFast
    {
        [Test]
        public void WordpressUrl_CannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new WordpressSettings(null));
        }

        [Test]
        public void WordpressUrl_CannotBeRelative()
        {
            Assert.Throws<ArgumentException>(() => new WordpressSettings(new Uri("/path", UriKind.Relative)));
        }

        [Test]
        public void WordpressUrl_ShouldAcceptAbsoluteUrl()
        {
            Assert.DoesNotThrow(() => new WordpressSettings(new Uri("http://some-url.com")));
        }

        [Test]
        public void Pages_IsNotNullByDefault()
        {
            new WordpressSettings(new Uri("http://some-url.com")).Pages.ShouldNotBeNull();
        }
    }
}