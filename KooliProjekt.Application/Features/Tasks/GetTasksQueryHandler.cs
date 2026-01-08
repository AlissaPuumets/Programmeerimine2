using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, OperationResult<object>>
    {
        private readonly ITasksRepository _tasksRepository;

        public GetTasksQueryHandler(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        public async Task<OperationResult<object>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var list = await _tasksRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = list.Id,
                Title = list.Title,
                Items = list.Id
                };

            return result;
        }
    }
}
