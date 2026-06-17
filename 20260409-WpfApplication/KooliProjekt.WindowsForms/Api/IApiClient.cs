using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<ToDoList>>> List(int page, int pageSize);
        Task<OperationResult> Save(ToDoList list);
        Task<OperationResult> Delete(int id);
    }
}