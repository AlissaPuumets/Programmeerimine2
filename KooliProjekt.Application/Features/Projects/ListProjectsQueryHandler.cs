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

namespace KooliProjekt.Application.Features.Projects
{
    public class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, OperationResult<PagedResult<Project>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProjectsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<Project>>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<Project>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            var query = _dbContext.Projects.AsQueryable();

            // Apply search filters
            if (!string.IsNullOrWhiteSpace(request.SearchName))
            {
                query = query.Where(p => p.Name.Contains(request.SearchName));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchStatus))
            {
                query = query.Where(p => p.Status.Contains(request.SearchStatus));
            }

            result.Value = await query
                .OrderBy(p => p.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
