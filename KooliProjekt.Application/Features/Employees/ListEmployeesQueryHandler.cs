using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Employees
{
    public class ListEmployeesQueryHandler : IRequestHandler<ListEmployeesQuery, OperationResult<PagedResult<Employee>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListEmployeesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<Employee>>> Handle(ListEmployeesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<Employee>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            var query = _dbContext.Employees.AsQueryable();

            // Apply search filters
            if (!string.IsNullOrWhiteSpace(request.SearchFirstName))
            {
                query = query.Where(e => e.FirstName.Contains(request.SearchFirstName));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchLastName))
            {
                query = query.Where(e => e.LastName.Contains(request.SearchLastName));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchEmail))
            {
                query = query.Where(e => e.Email.Contains(request.SearchEmail));
            }

            result.Value = await query
                .OrderBy(e => e.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
