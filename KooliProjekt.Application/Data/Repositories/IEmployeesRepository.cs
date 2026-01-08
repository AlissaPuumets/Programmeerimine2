using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IEmployeesRepository
    {
        global::System.Threading.Tasks.Task<Employee> GetByIdAsync(int id);
        global::System.Threading.Tasks.Task SaveAsync(Employee list);
        global::System.Threading.Tasks.Task DeleteAsync(Employee entity);
    }
}
