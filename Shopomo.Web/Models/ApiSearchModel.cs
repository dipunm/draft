namespace Shopomo.Web.Models
{
    public class ApiSearchModel : UriSearchModel
    {
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
    }
}