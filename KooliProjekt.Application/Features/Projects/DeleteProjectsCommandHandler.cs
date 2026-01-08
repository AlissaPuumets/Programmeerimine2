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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProjectsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Projects
                .Where(p => p.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
