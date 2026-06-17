using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.Tasks
{

    public class DeleteTasksCommandHandler : IRequestHandler<DeleteTasksCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTasksCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteTasksCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (task != null)
            {
                _dbContext.Tasks.Remove(task);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}