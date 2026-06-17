using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.ProjectMembers
{

    public class DeleteProjectMembersCommandHandler : IRequestHandler<DeleteProjectMembersCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectMembersCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteProjectMembersCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            var projectMember = await _dbContext.ProjectMembers.FirstOrDefaultAsync(pm => pm.Id == request.Id, cancellationToken);
            if (projectMember != null)
            {
                _dbContext.ProjectMembers.Remove(projectMember);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
