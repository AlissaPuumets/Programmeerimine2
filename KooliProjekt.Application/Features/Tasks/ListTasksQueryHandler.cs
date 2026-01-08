using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tasks
{
    public class ListTasksQueryHandler : IRequestHandler<ListTasksQuery, OperationResult<PagedResult<Data.Task>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListTasksQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Data.Task>>> Handle(ListTasksQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Data.Task>>();

            result.Value = await _dbContext
                .Tasks
                .OrderBy(list => list.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}