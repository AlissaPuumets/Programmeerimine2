using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectMembers
{
    public class GetProjectMembersQueryHandler : IRequestHandler<GetProjectMembersQuery, OperationResult<object>>
    {
        private readonly IProjectMembersRepository _projectMembersRepository;

        public GetProjectMembersQueryHandler(IProjectMembersRepository projectMembersRepository)
        {
            _projectMembersRepository = projectMembersRepository;
        }

        public async Task<OperationResult<object>> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var projectMember = await _projectMembersRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = projectMember.Id,
                ProjectId = projectMember.ProjectId,
                EmployeeId = projectMember.EmployeeId,
                RoleInProject = projectMember.RoleInProject
            };

            return result;
        }
    }
}
