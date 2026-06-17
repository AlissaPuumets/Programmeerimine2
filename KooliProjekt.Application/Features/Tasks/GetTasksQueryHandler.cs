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
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTasksQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<object>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Tasks
                .Where(task => task.Id == request.Id)
                .Select(list => new
                {
                    Id = list.Id,
                    Title = list.Title,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
