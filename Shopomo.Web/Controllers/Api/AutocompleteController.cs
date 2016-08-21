using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Shopomo.Web.Controllers.Api
{
    public class AutocompleteController : ApiController
    {
        private readonly ITermSearcher _termSearcher;

        public AutocompleteController(ITermSearcher termSearcher)
        {
            _termSearcher = termSearcher;
        }

        public async Task<IHttpActionResult> GetSuggestions(string term)
        {
            var productSuggestions = await _termSearcher.GetSuggestionsForTermAsync(term);
            var departments = await _termSearcher.GetTopDepartmentsForTermAsync(term);
            var suggestions = new //or whatever.
            {
                productSuggestions,
                departments
            };
            return Ok(suggestions);
        }
    }

    public interface ITermSearcher
    {
        Task<IEnumerable<object>> GetSuggestionsForTermAsync(string term);
        Task<IEnumerable<object>> GetTopDepartmentsForTermAsync(string term);
    }
}