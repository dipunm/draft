using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using ReturnNull.ValueProviders.Web.DataSources;
using ReturnNull.ValueProviders.Web.ModelBinding;
using Shopomo.ProductSearcher.Domain.Search;
using Shopomo.Web.Models.Binders;

namespace Shopomo.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Set up Querystring as a ModelBuilder ValueSource.
            DataSourceConfig.DataSources.Add("querystring", new QuerystringProvider());

            // Set up ModelBuilders for Search
            ModelBinders.Binders.Add(typeof(SearchModel), new ModelBinder<SearchModelBuilder, SearchModel>(new SearchModelBuilder()));
        }
    }
}