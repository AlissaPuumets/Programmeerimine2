using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Employees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEmployeesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<object>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Employees
                .Where(employee => employee.Id == request.Id)
                .Select(list => new
                {
                    Id = list.Id,
                    FirstName = list.FirstName,
                    LastName = list.LastName,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
