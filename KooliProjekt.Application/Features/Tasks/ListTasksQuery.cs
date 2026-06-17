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
        public string SearchTitle { get; set; }
        public string SearchStatus { get; set; }
        public string SearchPriority { get; set; }
        public int? SearchProjectId { get; set; }
    }
}
