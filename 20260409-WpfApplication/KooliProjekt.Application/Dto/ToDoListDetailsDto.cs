using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class ToDoListDetailsDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public IList<ToDoItemDto> Items { get; set; } = new List<ToDoItemDto>();
    }
}
