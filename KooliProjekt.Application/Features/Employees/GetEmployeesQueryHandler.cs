using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Employees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, OperationResult<object>>
    {
        private readonly IEmployeesRepository _employeesRepository;

        public GetEmployeesQueryHandler(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<OperationResult<object>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var employee = await _employeesRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                Role = employee.Role
            };

            return result;
        }
    }
}
