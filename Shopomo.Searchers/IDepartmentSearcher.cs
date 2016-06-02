using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopomo.Searchers
{
    public interface IDepartmentSearcher
    {
        Task<IEnumerable<object>> GetAllDepartmentsAsync();
    }
}