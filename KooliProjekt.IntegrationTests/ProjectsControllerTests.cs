using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Projects;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Projects/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Project>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_list()
        {
            // Arrange
            var url = "/api/Projects/Get/?id=1";
            
            var project = new Project { Name = "Test Project", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Status = "Active", Budget = 1000 };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<Project>>(url);
            
            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_list()
        {
            // Arrange
            var url = "/api/Projects/Get/?id=131";

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
            var url = "/api/Projects/Delete/";

            var project = new Project { Name = "Test Project", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Status = "Active", Budget = 1000 };
            
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = project.Id })
            };
            using var response = await Client.SendAsync(request);            
            var listFromDb = await DbContext.Projects
                .Where(p => p.Id == project.Id)
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
            var url = "/api/Projects/Delete/";

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
            var url = "/api/Projects/Save/";
            var project = new SaveProjectsCommand { Name = "Test Project", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Status = "Active", Budget = 1000 };

            // Act
            using var response = await Client.PostAsJsonAsync(url, project);
            var listFromDb = await DbContext.Projects
                .Where(p => p.Id == 1)
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
            var url = "/api/Projects/Save/";
            var project = new SaveProjectsCommand { Id = 10, Name = "Test Project", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Status = "Active", Budget = 1000 };

            // Act
            using var response = await Client.PostAsJsonAsync(url, project);
            var listFromDb = await DbContext.Projects
                .Where(p => p.Id == 10)
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
            var url = "/api/Projects/Save/";
            var project = new SaveProjectsCommand { Id = 0, Name = "" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, project);
            var listFromDb = await DbContext.Projects
                .Where(p => p.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}