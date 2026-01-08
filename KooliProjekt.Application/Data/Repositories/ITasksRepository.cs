using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    // 28.11
    // ToDo listide repository interface (Program.cs failis
    // tuleb see ka ära regada)
    public interface ITasksRepository
    {
        global::System.Threading.Tasks.Task<Data.Task> GetByIdAsync(int id);
        global::System.Threading.Tasks.Task SaveAsync(Data.Task list);
        global::System.Threading.Tasks.Task DeleteAsync(Data.Task entity);
    }
}
