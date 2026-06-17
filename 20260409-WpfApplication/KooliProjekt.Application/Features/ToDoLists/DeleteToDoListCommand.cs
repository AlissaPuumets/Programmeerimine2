using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ToDoLists
{
    // 15.11.2025
    // ToDoListi kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteToDoListCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}