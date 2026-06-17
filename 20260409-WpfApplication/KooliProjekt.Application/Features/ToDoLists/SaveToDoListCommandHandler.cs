using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ToDoLists
{
    public class SaveToDoListCommandHandler : IRequestHandler<SaveToDoListCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveToDoListCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveToDoListCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();           
            if(request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            var list = new ToDoList();
            if(request.Id == 0)
            {
                await _dbContext.ToDoLists.AddAsync(list);
            }
            else
            {
                list = await _dbContext.ToDoLists.FindAsync(request.Id);
                if (list == null)
                {
                    result.AddError("Cannot find list with ID " + request.Id);
                    return result;
                }
            }

            list.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
