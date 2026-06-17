using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class SaveProjectsCommandHandler : IRequestHandler<SaveProjectsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            Project project;
            if (request.Id == 0)
            {
                project = new Project();
                await _dbContext.Projects.AddAsync(project, cancellationToken);
            }
            else
            {
                project = await _dbContext.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
                if (project == null)
                {
                    result.AddError("Project not found");
                    return result;
                }
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;
            project.Status = request.Status;
            project.Budget = request.Budget;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
