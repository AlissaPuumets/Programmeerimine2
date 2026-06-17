using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectMembers;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectMembersControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/ProjectMembers/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ProjectMember>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_list()
        {
            // Arrange
            var url = "/api/ProjectMembers/Get/?id=1";
            
            var projectMember = new ProjectMember { ProjectId = 1, EmployeeId = 1, RoleInProject = "Developer" };
            await DbContext.AddAsync(projectMember);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ProjectMember>>(url);
            
            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_list()
        {
            // Arrange
            var url = "/api/ProjectMembers/Get/?id=131";

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
            var url = "/api/ProjectMembers/Delete/";

            var projectMember = new ProjectMember { ProjectId = 1, EmployeeId = 1, RoleInProject = "Developer" };
            
            await DbContext.AddAsync(projectMember);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = projectMember.Id })
            };
            using var response = await Client.SendAsync(request);            
            var listFromDb = await DbContext.ProjectMembers
                .Where(pm => pm.Id == projectMember.Id)
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
            var url = "/api/ProjectMembers/Delete/";

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
            var url = "/api/ProjectMembers/Save/";
            var projectMember = new SaveProjectMembersCommand { ProjectId = 1, EmployeeId = 1, RoleInProject = "Developer" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, projectMember);
            var listFromDb = await DbContext.ProjectMembers
                .Where(pm => pm.Id == 1)
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
            var url = "/api/ProjectMembers/Save/";
            var projectMember = new SaveProjectMembersCommand { Id = 10, ProjectId = 1, EmployeeId = 1, RoleInProject = "Developer" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, projectMember);
            var listFromDb = await DbContext.ProjectMembers
                .Where(pm => pm.Id == 10)
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
            var url = "/api/ProjectMembers/Save/";
            var projectMember = new SaveProjectMembersCommand { Id = 0, ProjectId = 0, EmployeeId = 0, RoleInProject = "" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, projectMember);
            var listFromDb = await DbContext.ProjectMembers
                .Where(pm => pm.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(listFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}