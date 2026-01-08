using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class EmployeesRepository : BaseRepository<Employee>, IEmployeesRepository
    {
        public EmployeesRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async global::System.Threading.Tasks.Task<Employee> GetByIdAsync(int id)
        {
            return await DbContext
                .Employees
                .Where(list => list.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
