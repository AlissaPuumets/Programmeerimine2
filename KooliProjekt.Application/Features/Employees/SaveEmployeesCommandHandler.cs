using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Employees
{
    public class SaveEmployeesCommandHandler : IRequestHandler<SaveEmployeesCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveEmployeesCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(SaveEmployeesCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            if (request.Id < 0)
            {
                result.AddError("Invalid ID");
                return result;
            }

            if (request.Id == 0)
            {
                // Add new employee
                var employee = new Employee
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Role = request.Role
                };
                await _dbContext.Employees.AddAsync(employee, cancellationToken);
            }
            else
            {
                // Update existing employee
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
                if (employee == null)
                {
                    result.AddError("Employee not found");
                    return result;
                }

                employee.FirstName = request.FirstName;
                employee.LastName = request.LastName;
                employee.Email = request.Email;
                employee.Phone = request.Phone;
                employee.Role = request.Role;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
