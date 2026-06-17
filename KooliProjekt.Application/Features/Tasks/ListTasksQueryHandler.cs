using System;
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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<Data.Task>>> Handle(ListTasksQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<Data.Task>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            var query = _dbContext.Tasks.AsQueryable();

            // Apply search filters
            if (!string.IsNullOrWhiteSpace(request.SearchTitle))
            {
                query = query.Where(t => t.Title.Contains(request.SearchTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchStatus))
            {
                query = query.Where(t => t.Status.Contains(request.SearchStatus));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchPriority))
            {
                query = query.Where(t => t.Priority.Contains(request.SearchPriority));
            }

            if (request.SearchProjectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == request.SearchProjectId.Value);
            }

            result.Value = await query
                .OrderBy(list => list.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}