using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.ProjectMembers
{
    public class GetProjectMembersQueryHandler : IRequestHandler<GetProjectMembersQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProjectMembersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<object>> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .ProjectMembers
                .Where(projectmember => projectmember.Id == request.Id)
                .Select(list => new
                {
                    Id = list.Id,
                    RoleInProject = list.RoleInProject,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
