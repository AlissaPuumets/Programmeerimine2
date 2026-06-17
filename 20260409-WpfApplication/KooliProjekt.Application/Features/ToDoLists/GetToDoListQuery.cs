using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ToDoLists
{
    // 16.01.2026 - tagastab ToDoListDetailsDto
    [ExcludeFromCodeCoverage]
    public class GetToDoListQuery : IRequest<OperationResult<ToDoListDetailsDto>>
    {
        public int Id { get; set; }
    }
}
