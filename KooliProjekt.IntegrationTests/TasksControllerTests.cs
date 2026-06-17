using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ApplicationTask = KooliProjekt.Application.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TasksControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Tasks/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ApplicationTask>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_list()
        {
            // Arrange
            var url = "/api/Tasks/Get/?id=1";
            
            var task = new ApplicationTask { Title = "Test Task", ProjectId = 1, AssignedTo = 1, Status = "New", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Priority = "High" };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ApplicationTask>>(url);
            
            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_list()
        {
            // Arrange
            var url = "/api/Tasks/Get/?id=131";

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
            var url = "/api/Tasks/Delete/";

            var task = new ApplicationTask { Title = "Test Task", ProjectId = 1, AssignedTo = 1, Status = "New", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Priority = "High" };
            
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = task.Id })
            };
            using var response = await Client.SendAsync(request);            
            var listFromDb = await DbContext.Tasks
                .Where(t => t.Id == task.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_shouldwork_with_missing_list()
        {
            // Arrange
            var url = "/api/Tasks/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id  = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_list()
        {
            // Arrange
            var url = "/api/Tasks/Save/";
            var task = new SaveTasksCommand { 
                Title = "Test Task",
                ProjectId = 1,
                AssignedTo = 1,
                Status = "New",
                Priority = "Normal",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, task);
            var listFromDb = await DbContext.Tasks
                .Where(t => t.Title == "Test Task")
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_list()
        {
            // Arrange
            var url = "/api/Tasks/Save/";
            var task = new SaveTasksCommand { Id = 10, Title = "Test Task" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, task);
            var listFromDb = await DbContext.Tasks
                .Where(t => t.Id == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_list()
        {
            // Arrange
            var url = "/api/Tasks/Save/";
            var task = new SaveTasksCommand { Id = 0, Title = "" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, task);
            var listFromDb = await DbContext.Tasks
                .Where(t => t.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}