using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.Projects
{

    public class DeleteProjectsCommandHandler : IRequestHandler<DeleteProjectsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteProjectsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
