using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tasks
{
    public class GetTasksQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
