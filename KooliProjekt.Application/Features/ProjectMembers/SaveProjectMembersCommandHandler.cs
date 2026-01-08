using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectMembers
{
    public class SaveProjectMembersCommandHandler : IRequestHandler<SaveProjectMembersCommand, OperationResult>
    {
        private readonly IProjectMembersRepository _projectMembersRepository;

        public SaveProjectMembersCommandHandler(IProjectMembersRepository projectMembersRepository)
        {
            _projectMembersRepository = projectMembersRepository;
        }

        public async Task<OperationResult> Handle(SaveProjectMembersCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var projectMember = new ProjectMember();
            if(request.Id != 0)
            {
                projectMember = await _projectMembersRepository.GetByIdAsync(request.Id);
            }

            projectMember.ProjectId = request.ProjectId;
            projectMember.EmployeeId = request.EmployeeId;
            projectMember.RoleInProject = request.RoleInProject;

            await _projectMembersRepository.SaveAsync(projectMember);

            return result;
        }
    }
}
