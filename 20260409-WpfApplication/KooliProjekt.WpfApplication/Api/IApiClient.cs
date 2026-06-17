using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Employee>>> List(int page, int pageSize);
        Task<OperationResult> Save(Employee list);
        Task<OperationResult> Delete(int id);
    }
}