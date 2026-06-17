using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.ToDoLists
{
    public class DeleteToDoListCommandHandler : IRequestHandler<DeleteToDoListCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteToDoListCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteToDoListCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            // Kustutamine üle relatsioonide (vihje: CASCADE DELETE)
            //await _dbContext
            //    .ToDoLists
            //    .Where(t => t.Id == request.Id)
            //    .ExecuteDeleteAsync();  <-- InMemory ei toeta veel ExecuteDeleteAsync meetodit

            var list = await _dbContext
                .ToDoLists
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            
            if(list == null)
            {
                return result;
            }

            _dbContext.ToDoItems.RemoveRange(list.Items);
            _dbContext.ToDoLists.Remove(list);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}