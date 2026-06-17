using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.ToDoLists
{
    // 16.01.2026 - ToDoListDetailsDto
    public class GetToDoListQueryHandler : IRequestHandler<GetToDoListQuery, OperationResult<ToDoListDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetToDoListQueryHandler(ApplicationDbContext dbContext)
        {
            if(dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<ToDoListDetailsDto>> Handle(GetToDoListQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }            

            var result = new OperationResult<ToDoListDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .ToDoLists
                .Include(list => list.Items)
                .Where(list => list.Id == request.Id)
                .Select(list => new ToDoListDetailsDto
                {
                    Id = list.Id,
                    Title = list.Title,
                    Items = list.Items.Select(item => new ToDoItemDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        IsDone = item.IsDone
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
