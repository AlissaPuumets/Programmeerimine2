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

namespace KooliProjekt.Application.Features.ProjectMembers
{
    public class ListProjectMembersQueryHandler : IRequestHandler<ListProjectMembersQuery, OperationResult<PagedResult<ProjectMember>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProjectMembersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<ProjectMember>>> Handle(ListProjectMembersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<ProjectMember>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            var query = _dbContext.ProjectMembers.AsQueryable();

            // Apply search filters
            if (request.SearchProjectId.HasValue)
            {
                query = query.Where(pm => pm.ProjectId == request.SearchProjectId.Value);
            }

            if (request.SearchEmployeeId.HasValue)
            {
                query = query.Where(pm => pm.EmployeeId == request.SearchEmployeeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchRoleInProject))
            {
                query = query.Where(pm => pm.RoleInProject.Contains(request.SearchRoleInProject));
            }

            result.Value = await query
                .OrderBy(pm => pm.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
