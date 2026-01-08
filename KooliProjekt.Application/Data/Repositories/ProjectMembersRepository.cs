using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class ProjectMembersRepository : BaseRepository<ProjectMember>, IProjectMembersRepository
    {
        public ProjectMembersRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async global::System.Threading.Tasks.Task<ProjectMember> GetByIdAsync(int id)
        {
            return await DbContext
                .ProjectMembers
                .Where(list => list.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
