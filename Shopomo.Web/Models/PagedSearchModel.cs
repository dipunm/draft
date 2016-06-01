namespace Shopomo.Web.Models
{
    public class PagedSearchModel : SearchModel
    {
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
    }
}