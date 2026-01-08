using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Employees
{
    public class SaveEmployeesCommandHandler : IRequestHandler<SaveEmployeesCommand, OperationResult>
    {
        private readonly IEmployeesRepository _employeesRepository;

        public SaveEmployeesCommandHandler(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<OperationResult> Handle(SaveEmployeesCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var employee = new Employee();
            if(request.Id != 0)
            {
                employee = await _employeesRepository.GetByIdAsync(request.Id);
            }

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Email = request.Email;
            employee.Phone = request.Phone;
            employee.Role = request.Role;

            await _employeesRepository.SaveAsync(employee);

            return result;
        }
    }
}
