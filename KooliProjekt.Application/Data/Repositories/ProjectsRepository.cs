using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    // 28.11
    // ToDo listide repository klass
    public class ProjectsRepository : BaseRepository<Project>, IProjectsRepository
    {
        public ProjectsRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        // Lisa siia spetsiifilisemad meetodid,
        // mis on seotud ToDoListidega

        // BaseRepository ei tea, et Get peab tooma kaasa ka Itemsid
        public override async global::System.Threading.Tasks.Task<Project> GetByIdAsync(int id)
        {
            return await DbContext
                .Projects
                .Where(list => list.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
