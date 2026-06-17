using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
    }
}