using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ToDoLists
{
    public class ListToDoListsQueryHandler : IRequestHandler<ListToDoListsQuery, OperationResult<PagedResult<ToDoList>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListToDoListsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<ToDoList>>> Handle(ListToDoListsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<ToDoList>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<ToDoList>();
                return result;
            }

            var query = _dbContext.ToDoLists.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(list => list.Title.Contains(request.Title));
            }

            if(request.IsDone.HasValue)
            {
                if (request.IsDone.Value)
                {
                    query = query.Where(list => list.Items.All(item => item.IsDone));
                }
                else
                {
                    query = query.Where(list => list.Items.Any(item => !item.IsDone));
                }
            }

            result.Value = await query
                .OrderBy(list => list.Title)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}