using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ToDoLists;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TodoListsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/ToDoLists/List/?page=0&pageSize=0";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ToDoList>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_list()
        {
            // Arrange
            var url = "/api/ToDoLists/Get/?id=1";

            var todoList = new ToDoList { Title = "Test List" };
            await DbContext.AddAsync(todoList);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ToDoList>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_list()
        {
            // Arrange
            var url = "/api/ToDoLists/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_list()
        {
            // Arrange
            var url = "/api/ToDoLists/Delete";
            var list = new ToDoList 
            {
                Title = "New List" ,
                Items = new List<ToDoItem>
                {
                    new ToDoItem { Title = "Test Item 1" },
                    new ToDoItem { Title = "Test Item 2" }
                }
            };

            await DbContext.AddAsync(list);
            await DbContext.SaveChangesAsync();

            var command = new DeleteToDoListCommand { Id = list.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdList = await DbContext.ToDoLists.FirstOrDefaultAsync(l => l.Id == 1);
            Assert.Null(createdList);
        }

        [Fact]
        public async Task Delete_should_handle_missing_list()
        {
            // Arrange
            var url = "/api/ToDoLists/Delete";

            var command = new DeleteToDoListCommand { Id = 101 };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdList = await DbContext.ToDoLists.FirstOrDefaultAsync(l => l.Id == 1);
            Assert.Null(createdList);
        }

        [Fact]
        public async Task Save_should_add_new_list()
        {
            // Arrange
            var url = "/api/ToDoLists/Save";
            var command = new SaveToDoListCommand { Title = "New List" };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            var body = await response.Content.ReadAsStringAsync();
            //response.EnsureSuccessStatusCode();
            var createdList = await DbContext.ToDoLists.FirstOrDefaultAsync(l => l.Title == "New List");
            Assert.NotNull(createdList);
        }

        [Fact]
        public async Task Save_should_not_update_missing_list()
        {
            // Arrange
            var url = "/api/ToDoLists/Save";
            var command = new SaveToDoListCommand { Id = 101, Title = "New List" };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var createdList = await DbContext.ToDoLists.FirstOrDefaultAsync(l => l.Id == 101);
            Assert.Null(createdList);
        }

        [Fact]
        public async Task Save_should_not_update_invalid_list()
        {
            // Arrange
            var url = "/api/ToDoLists/Save";
            var command = new SaveToDoListCommand { Title = "" };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var createdList = await DbContext.ToDoLists.FirstOrDefaultAsync(l => l.Id == 1);
            Assert.Null(createdList);
        }
    }
}