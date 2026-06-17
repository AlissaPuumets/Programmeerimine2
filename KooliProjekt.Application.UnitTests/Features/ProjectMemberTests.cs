using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectMembers;
using Xunit;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ProjectMemberTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectMembersQueryHandler(null);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetProjectMembersQuery)null;
            var handler = new GetProjectMembersQueryHandler(DbContext);

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
            var query = new GetProjectMembersQuery { Id = id };
            var handler = new GetProjectMembersQueryHandler(GetFaultyDbContext());

            // First create project and employee
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var projectMember = new ProjectMember { ProjectId = project.Id, EmployeeId = employee.Id, RoleInProject = "Developer" };
            await DbContext.ProjectMembers.AddAsync(projectMember);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_return_existing_project_member()
        {
            // Arrange
            var query = new GetProjectMembersQuery { Id = 1 };
            var handler = new GetProjectMembersQueryHandler(DbContext);

            // First create project and employee
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var projectMember = new ProjectMember { ProjectId = project.Id, EmployeeId = employee.Id, RoleInProject = "Developer" };
            await DbContext.ProjectMembers.AddAsync(projectMember);
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
        public async System.Threading.Tasks.Task Get_should_return_null_when_project_member_does_not_exist(int id)
        {
            // Arrange
            var query = new GetProjectMembersQuery { Id = id };
            var handler = new GetProjectMembersQueryHandler(DbContext);

            // First create project and employee
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var projectMember = new ProjectMember { ProjectId = project.Id, EmployeeId = employee.Id, RoleInProject = "Developer" };
            await DbContext.ProjectMembers.AddAsync(projectMember);
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
                new DeleteProjectMembersCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteProjectMembersCommand)null;
            var handler = new DeleteProjectMembersCommandHandler(DbContext);

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
            var query = new DeleteProjectMembersCommand { Id = id };
            var faultyDbContext = GetFaultyDbContext();
            var handler = new DeleteProjectMembersCommandHandler(faultyDbContext);

            // First create project and employee
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var projectMember = new ProjectMember { ProjectId = project.Id, EmployeeId = employee.Id, RoleInProject = "Developer" };
            await DbContext.ProjectMembers.AddAsync(projectMember);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_remove_existing_project_member()
        {
            // Arrange
            var query = new DeleteProjectMembersCommand { Id = 1 };
            var handler = new DeleteProjectMembersCommandHandler(DbContext);

            // First create project and employee
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var projectMember = new ProjectMember { ProjectId = project.Id, EmployeeId = employee.Id, RoleInProject = "Developer" };

            await DbContext.ProjectMembers.AddAsync(projectMember);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var projectMemberTest = await DbContext.ProjectMembers.FindAsync(query.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(projectMemberTest);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_not_fail_when_project_member_does_not_exists()
        {
            // Arrange
            var query = new DeleteProjectMembersCommand { Id = 101 };
            var handler = new DeleteProjectMembersCommandHandler(DbContext);

            // First create project and employee
            var project = new Project { Name = "Test Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var projectMember = new ProjectMember { ProjectId = project.Id, EmployeeId = employee.Id, RoleInProject = "Developer" };

            await DbContext.ProjectMembers.AddAsync(projectMember);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var projectMemberTest = await DbContext.ProjectMembers.FindAsync(query.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(projectMemberTest);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_project_id()
        {
            // Arrange
            var project1 = new Project { Name = "Project 1", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            var project2 = new Project { Name = "Project 2", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 3000m };
            await DbContext.Projects.AddAsync(project1);
            await DbContext.Projects.AddAsync(project2);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var query = new ListProjectMembersQuery { Page = 1, PageSize = 10, SearchProjectId = project1.Id };
            var handler = new ListProjectMembersQueryHandler(DbContext);

            await DbContext.ProjectMembers.AddAsync(new ProjectMember { ProjectId = project1.Id, EmployeeId = employee.Id, RoleInProject = "Developer" });
            await DbContext.ProjectMembers.AddAsync(new ProjectMember { ProjectId = project2.Id, EmployeeId = employee.Id, RoleInProject = "Manager" });
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
        public async System.Threading.Tasks.Task List_should_search_by_employee_id()
        {
            // Arrange
            var project = new Project { Name = "Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee1 = new Employee { FirstName = "John", LastName = "Doe", Email = "john@test.com", Phone = "1234567890", Role = "Developer" };
            var employee2 = new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Phone = "9876543210", Role = "Manager" };
            await DbContext.Employees.AddAsync(employee1);
            await DbContext.Employees.AddAsync(employee2);
            await DbContext.SaveChangesAsync();

            var query = new ListProjectMembersQuery { Page = 1, PageSize = 10, SearchEmployeeId = employee1.Id };
            var handler = new ListProjectMembersQueryHandler(DbContext);

            await DbContext.ProjectMembers.AddAsync(new ProjectMember { ProjectId = project.Id, EmployeeId = employee1.Id, RoleInProject = "Developer" });
            await DbContext.ProjectMembers.AddAsync(new ProjectMember { ProjectId = project.Id, EmployeeId = employee2.Id, RoleInProject = "Manager" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal(employee1.Id, result.Value.Results.First().EmployeeId);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_role_in_project()
        {
            // Arrange
            var project = new Project { Name = "Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee1 = new Employee { FirstName = "John", LastName = "Doe", Email = "john@test.com", Phone = "1234567890", Role = "Developer" };
            var employee2 = new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Phone = "9876543210", Role = "Manager" };
            await DbContext.Employees.AddAsync(employee1);
            await DbContext.Employees.AddAsync(employee2);
            await DbContext.SaveChangesAsync();

            var query = new ListProjectMembersQuery { Page = 1, PageSize = 10, SearchRoleInProject = "Manager" };
            var handler = new ListProjectMembersQueryHandler(DbContext);

            await DbContext.ProjectMembers.AddAsync(new ProjectMember { ProjectId = project.Id, EmployeeId = employee1.Id, RoleInProject = "Developer" });
            await DbContext.ProjectMembers.AddAsync(new ProjectMember { ProjectId = project.Id, EmployeeId = employee2.Id, RoleInProject = "Manager" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Manager", result.Value.Results.First().RoleInProject);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_return_empty_when_search_returns_no_results()
        {
            // Arrange
            var project = new Project { Name = "Project", Description = "Test", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Status = "Active", Budget = 5000m };
            await DbContext.Projects.AddAsync(project);

            var employee = new Employee { FirstName = "John", LastName = "Doe", Email = "john@test.com", Phone = "1234567890", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            var query = new ListProjectMembersQuery { Page = 1, PageSize = 10, SearchRoleInProject = "NonExistent" };
            var handler = new ListProjectMembersQueryHandler(DbContext);

            await DbContext.ProjectMembers.AddAsync(new ProjectMember { ProjectId = project.Id, EmployeeId = employee.Id, RoleInProject = "Developer" });
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
