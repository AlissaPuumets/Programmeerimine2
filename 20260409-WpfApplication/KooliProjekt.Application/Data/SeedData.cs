using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    // 15.11.2025
    // SeedData klass andmete genereerimiseks
    [ExcludeFromCodeCoverage]
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IList<ToDoList> _toDoLists = new List<ToDoList>();

        public SeedData(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        // Generate meetod koordineerib andmete genereerimist
        public void Generate()
        {
            // Ära tee midagi kui andmed on juba olemas
            if (_dbContext.ToDoLists.Any())
            {
                return;
            }

            GenerateToDoLists();
            GenerateToDoItems();

            _dbContext.SaveChanges();
        }

        private void GenerateToDoLists()
        {
            for(var i = 0; i < 10; i++)
            {
                var toDoList = new ToDoList
                {
                    Title = $"List {i + 1}"
                };

                _toDoLists.Add(toDoList);
            }

            _dbContext.ToDoLists.AddRange(_toDoLists);
        }

        private void GenerateToDoItems()
        {
            foreach(var list in _toDoLists)
            {
                for(var j = 0; j < 5; j++)
                {
                    var toDoItem = new ToDoItem
                    {
                        Title = $"{list.Title}, Item {j + 1}",
                        ToDoListId = list.Id
                    };
                    
                    list.Items.Add(toDoItem);
                }
            }
        }
    }
}