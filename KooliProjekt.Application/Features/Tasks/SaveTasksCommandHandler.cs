using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Tasks
{
    public class SaveTasksCommandHandler : IRequestHandler<SaveTasksCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTasksCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTasksCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            Data.Task task;
            if (request.Id == 0)
            {
                task = new Data.Task();
                await _dbContext.Tasks.AddAsync(task, cancellationToken);
            }
            else
            {
                task = await _dbContext.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);
                if (task == null)
                {
                    result.AddError("Task not found");
                    return result;
                }
            }

            task.ProjectId = request.ProjectId;
            task.Title = request.Title;
            task.Description = request.Description;
            task.AssignedTo = request.AssignedTo;
            task.Status = request.Status;
            task.StartDate = request.StartDate;
            task.EndDate = request.EndDate;
            task.Priority = request.Priority;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
