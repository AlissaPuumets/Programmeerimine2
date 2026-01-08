using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IProjectsRepository
    {
        global::System.Threading.Tasks.Task<Project> GetByIdAsync(int id);
        global::System.Threading.Tasks.Task SaveAsync(Project list);
        global::System.Threading.Tasks.Task DeleteAsync(Project entity);
    }
}
