using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Employee>>> List(int page, int pageSize);
        Task<OperationResult> Save(Employee list);
        Task<OperationResult> Delete(int id);
    }
}