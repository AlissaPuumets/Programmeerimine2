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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteEmployeesCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
