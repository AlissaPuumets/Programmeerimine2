using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IProjectMembersRepository
    {
        global::System.Threading.Tasks.Task<ProjectMember> GetByIdAsync(int id);
        global::System.Threading.Tasks.Task SaveAsync(ProjectMember list);
        global::System.Threading.Tasks.Task DeleteAsync(ProjectMember entity);
    }
}
