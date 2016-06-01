using System;
using System.Web;
using System.Web.Mvc;

namespace Shopomo.Web.ViewExtensions
{
    public static class UrlHelperExtensions
    {
        public static Uri FromCurrent(this UrlHelper urlHelper, Action<UriBuilder> changes = null)
        {
            var url = urlHelper.RequestContext.HttpContext.Request.Url;
            if (url == null)
                throw new InvalidOperationException("Could not determine the current Url.");

            var builder = new UriBuilder(url);
            changes?.Invoke(builder);
            return builder.Uri;
        }
    }

    public static class UriBuilderExtensions
    {
        public static void AddQuery(this UriBuilder builder, string key, object value)
        {
            var q = HttpUtility.ParseQueryString(builder.Query);
            q.Remove(key);
            q.Add(key, value.ToString());
            builder.Query = q.ToString();
        }

        public static void RemoveQuery(this UriBuilder builder, string key)
        {
            var q = HttpUtility.ParseQueryString(builder.Query);
            q.Remove(key);
            builder.Query = q.ToString();
        }
    }
}