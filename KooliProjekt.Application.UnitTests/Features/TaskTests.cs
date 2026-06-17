using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Tasks;
using Xunit;
using KooliProjekt.Application.UnitTests;
using TaskEntity = KooliProjekt.Application.Data.Task;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class TaskTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetTasksQueryHandler(null);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetTasksQuery)null;
            var handler = new GetTasksQueryHandler(DbContext);

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
            var query = new GetTasksQuery { Id = id };
            var handler = new GetTasksQueryHandler(GetFaultyDbContext());

            // First create a project for the task
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new TaskEntity { Title = "Test Task", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" };
            await DbContext.Tasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_return_existing_task()
        {
            // Arrange
            var query = new GetTasksQuery { Id = 1 };
            var handler = new GetTasksQueryHandler(DbContext);

            // First create a project for the task
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new TaskEntity { Title = "Test Task", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" };
            await DbContext.Tasks.AddAsync(task);
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
        public async System.Threading.Tasks.Task Get_should_return_null_when_task_does_not_exist(int id)
        {
            // Arrange
            var query = new GetTasksQuery { Id = id };
            var handler = new GetTasksQueryHandler(DbContext);

            // First create a project for the task
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new TaskEntity { Title = "Test Task", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" };
            await DbContext.Tasks.AddAsync(task);
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
                new DeleteTasksCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteTasksCommand)null;
            var handler = new DeleteTasksCommandHandler(DbContext);

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
            var query = new DeleteTasksCommand { Id = id };
            var faultyDbContext = GetFaultyDbContext();
            var handler = new DeleteTasksCommandHandler(faultyDbContext);

            // First create a project for the task
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new TaskEntity { Title = "Test Task", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" };
            await DbContext.Tasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_remove_existing_task()
        {
            // Arrange
            var query = new DeleteTasksCommand { Id = 1 };
            var handler = new DeleteTasksCommandHandler(DbContext);

            // First create a project for the task
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new TaskEntity { Title = "Test Task", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" };

            await DbContext.Tasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var taskTest = await DbContext.Tasks.FindAsync(query.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(taskTest);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_not_fail_when_task_does_not_exists()
        {
            // Arrange
            var query = new DeleteTasksCommand { Id = 101 };
            var handler = new DeleteTasksCommandHandler(DbContext);

            // First create a project for the task
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new TaskEntity { Title = "Test Task", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" };

            await DbContext.Tasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var taskTest = await DbContext.Tasks.FindAsync(query.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(taskTest);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_title()
        {
            // Arrange
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var query = new ListTasksQuery { Page = 1, PageSize = 10, SearchTitle = "Backend" };
            var handler = new ListTasksQueryHandler(DbContext);

            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Backend API", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Frontend UI", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Contains("Backend", result.Value.Results.First().Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_status()
        {
            // Arrange
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var query = new ListTasksQuery { Page = 1, PageSize = 10, SearchStatus = "Completed" };
            var handler = new ListTasksQueryHandler(DbContext);

            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Backend API", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Completed", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Frontend UI", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
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
        public async System.Threading.Tasks.Task List_should_search_by_priority()
        {
            // Arrange
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var query = new ListTasksQuery { Page = 1, PageSize = 10, SearchPriority = "Low" };
            var handler = new ListTasksQueryHandler(DbContext);

            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Backend API", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "Low" });
            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Frontend UI", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Low", result.Value.Results.First().Priority);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_project_id()
        {
            // Arrange
            var project1 = new Project { Name = "Project 1", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            var project2 = new Project { Name = "Project 2", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 3000m };
            await DbContext.Projects.AddAsync(project1);
            await DbContext.Projects.AddAsync(project2);
            await DbContext.SaveChangesAsync();

            var query = new ListTasksQuery { Page = 1, PageSize = 10, SearchProjectId = project1.Id };
            var handler = new ListTasksQueryHandler(DbContext);

            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Task 1", Description = "Test", ProjectId = project1.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Task 2", Description = "Test", ProjectId = project2.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal(project1.Id, result.Value.Results.First().ProjectId);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_return_empty_when_search_returns_no_results()
        {
            // Arrange
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var query = new ListTasksQuery { Page = 1, PageSize = 10, SearchTitle = "NonExistent" };
            var handler = new ListTasksQueryHandler(DbContext);

            await DbContext.Tasks.AddAsync(new TaskEntity { Title = "Backend API", Description = "Test", ProjectId = project.Id, AssignedTo = 1, Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), Priority = "High" });
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
