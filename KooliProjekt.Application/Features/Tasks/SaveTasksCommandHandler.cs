using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tasks
{
    public class SaveTasksCommandHandler : IRequestHandler<SaveTasksCommand, OperationResult>
    {
        private readonly ITasksRepository _tasksRepository;

        public SaveTasksCommandHandler(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        public async Task<OperationResult> Handle(SaveTasksCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Data.Task();
            if(request.Id != 0)
            {
                list = await _tasksRepository.GetByIdAsync(request.Id);
            }

            list.Title = request.Title;

            await _tasksRepository.SaveAsync(list);

            return result;
        }
    }
}
