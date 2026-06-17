using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectMembers
{
    public class SaveProjectMembersCommandHandler : IRequestHandler<SaveProjectMembersCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectMembersCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectMembersCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            ProjectMember projectMember;
            if (request.Id == 0)
            {
                projectMember = new ProjectMember();
                await _dbContext.ProjectMembers.AddAsync(projectMember, cancellationToken);
            }
            else
            {
                projectMember = await _dbContext.ProjectMembers.FindAsync(new object[] { request.Id }, cancellationToken);
                if (projectMember == null)
                {
                    result.AddError("Project member not found");
                    return result;
                }
            }

            projectMember.ProjectId = request.ProjectId;
            projectMember.EmployeeId = request.EmployeeId;
            projectMember.RoleInProject = request.RoleInProject;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
