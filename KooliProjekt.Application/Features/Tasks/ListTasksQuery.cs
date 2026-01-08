using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tasks
{
    public class ListTasksQuery : IRequest<OperationResult<PagedResult<Task>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
