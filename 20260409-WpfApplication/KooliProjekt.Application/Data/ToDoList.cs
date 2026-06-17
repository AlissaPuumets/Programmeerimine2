using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class ToDoList
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public IList<ToDoItem> Items { get; set; }

        // 15.11.2025
        // Väärtuste Items automaatselt ära (väldime mujal NullReferenceException)
        public ToDoList()
        {
            Items = new List<ToDoItem>();
        }
    }
}
