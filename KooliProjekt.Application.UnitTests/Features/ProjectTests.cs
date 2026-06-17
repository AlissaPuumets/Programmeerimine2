using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Projects;
using Xunit;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProjectTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectsQueryHandler(null);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetProjectsQuery)null;
            var handler = new GetProjectsQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async System.Threading.Tasks.Task Get_should_return_null_when_request_id_is_null_or_negative(int id)
        {
            // Arrange
            var query = new GetProjectsQuery { Id = id };
            var handler = new GetProjectsQueryHandler(GetFaultyDbContext());

            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_return_existing_project()
        {
            // Arrange
            var query = new GetProjectsQuery { Id = 1 };
            var handler = new GetProjectsQueryHandler(DbContext);

            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData(101)]
        public async System.Threading.Tasks.Task Get_should_return_null_when_project_does_not_exist(int id)
        {
            // Arrange
            var query = new GetProjectsQuery { Id = id };
            var handler = new GetProjectsQueryHandler(DbContext);

            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        // 23.01.2026
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteProjectsCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteProjectsCommand)null;
            var handler = new DeleteProjectsCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async System.Threading.Tasks.Task Delete_should_return_when_request_id_is_null_or_negative(int id)
        {
            // Arrange
            var query = new DeleteProjectsCommand { Id = id };
            var faultyDbContext = GetFaultyDbContext();
            var handler = new DeleteProjectsCommandHandler(faultyDbContext);

            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_remove_existing_project()
        {
            // Arrange
            var query = new DeleteProjectsCommand { Id = 1 };
            var handler = new DeleteProjectsCommandHandler(DbContext);

            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };

            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var projectTest = await DbContext.Projects.FindAsync(query.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(projectTest);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_not_fail_when_project_does_not_exists()
        {
            // Arrange
            var query = new DeleteProjectsCommand { Id = 101 };
            var handler = new DeleteProjectsCommandHandler(DbContext);

            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };

            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var projectTest = await DbContext.Projects.FindAsync(query.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(projectTest);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_name()
        {
            // Arrange
            var query = new ListProjectsQuery { Page = 1, PageSize = 10, SearchName = "Website" };
            var handler = new ListProjectsQueryHandler(DbContext);

            await DbContext.Projects.AddAsync(new Project { Name = "Website Redesign", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m });
            await DbContext.Projects.AddAsync(new Project { Name = "Mobile App", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 3000m });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Contains("Website", result.Value.Results.First().Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_status()
        {
            // Arrange
            var query = new ListProjectsQuery { Page = 1, PageSize = 10, SearchStatus = "Completed" };
            var handler = new ListProjectsQueryHandler(DbContext);

            await DbContext.Projects.AddAsync(new Project { Name = "Website Redesign", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Completed", Budget = 5000m });
            await DbContext.Projects.AddAsync(new Project { Name = "Mobile App", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 3000m });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Completed", result.Value.Results.First().Status);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_return_empty_when_search_returns_no_results()
        {
            // Arrange
            var query = new ListProjectsQuery { Page = 1, PageSize = 10, SearchName = "NonExistent" };
            var handler = new ListProjectsQueryHandler(DbContext);

            await DbContext.Projects.AddAsync(new Project { Name = "Website Redesign", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }
    }
}
