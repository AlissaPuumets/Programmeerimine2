using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectMembers
{
    public class DeleteProjectMembersCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
