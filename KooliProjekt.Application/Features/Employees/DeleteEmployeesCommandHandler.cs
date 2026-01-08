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

    public class DeleteEmployeesCommandHandler : IRequestHandler<DeleteEmployeesCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteEmployeesCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteEmployeesCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Employees
                .Where(e => e.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
