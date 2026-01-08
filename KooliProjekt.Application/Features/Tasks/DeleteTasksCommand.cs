using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tasks
{
    public class DeleteTasksCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}