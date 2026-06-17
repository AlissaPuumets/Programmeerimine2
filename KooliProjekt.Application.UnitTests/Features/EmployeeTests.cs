using KooliProjekt.Application.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Employees;
using Microsoft.EntityFrameworkCore;
using Xunit;
using TaskEntity = KooliProjekt.Application.Data.Task;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class EmployeeTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetEmployeesQueryHandler(null);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetEmployeesQuery)null;
            var handler = new GetEmployeesQueryHandler(DbContext);

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
            var query = new GetEmployeesQuery { Id = id };
            var handler = new GetEmployeesQueryHandler(GetFaultyDbContext());

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_should_return_existing_todo_list()
        {
            // Arrange
            var query = new GetEmployeesQuery { Id = 1 };
            var handler = new GetEmployeesQueryHandler(DbContext);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
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
        public async System.Threading.Tasks.Task Get_should_return_null_when_todo_list_does_not_exist(int id)
        {
            // Arrange
            var query = new GetEmployeesQuery { Id = id };
            var handler = new GetEmployeesQueryHandler(DbContext);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListEmployeesQueryHandler(null);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListEmployeesQuery)null;
            var handler = new ListEmployeesQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(4, -10)]
        [InlineData(5, -5)]
        [InlineData(0, 0)]
        [InlineData(-5, -10)]
        public async System.Threading.Tasks.Task List_should_return_null_when_page_or_page_size_is_zero_or_negative(int page, int pageSize)
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = page, PageSize = pageSize };
            var handler = new ListEmployeesQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_return_page_of_todo_lists()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 5 };
            var handler = new ListEmployeesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                var employee = new Employee { FirstName = $"Employee{i}", LastName = "Test", Email = $"employee{i}@example.com", Phone = "123456789", Role = "Developer" };
                await DbContext.Employees.AddAsync(employee);
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_return_empty_result_if_todo_lists_doesnt_exist()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 5 };
            var handler = new ListEmployeesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteEmployeesCommandHandler(null);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteEmployeesCommand)null;
            var handler = new DeleteEmployeesCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async System.Threading.Tasks.Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            // Arrange
            var query = new DeleteEmployeesCommand { Id = id };
            var handler = new DeleteEmployeesCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_delete_existing_todo_list()
        {
            // Arrange
            var query = new DeleteEmployeesCommand { Id = 1 };
            var handler = new DeleteEmployeesCommandHandler(DbContext);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Employees.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async System.Threading.Tasks.Task Delete_should_work_with_not_existing_list()
        {
            // Arrange
            var query = new DeleteEmployeesCommand { Id = 1034 };
            var handler = new DeleteEmployeesCommandHandler(DbContext);

            var employee = new Employee { FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveEmployeesCommandHandler(null);
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveEmployeesCommand)null;
            var handler = new SaveEmployeesCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async System.Threading.Tasks.Task Save_should_return_if_id_is_negative()
        {
            // Arrange
            var request = new SaveEmployeesCommand { Id = -10 };
            var handler = new SaveEmployeesCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async System.Threading.Tasks.Task Save_should_add_new_list()
        {
            // Arrange
            var request = new SaveEmployeesCommand { Id = 0, FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            var handler = new SaveEmployeesCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedList = await DbContext.Employees.SingleOrDefaultAsync(list => list.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedList);
            Assert.Equal(1, savedList.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task Save_should_update_existing_list()
        {
            // Arrange
            var request = new SaveEmployeesCommand { Id = 1, FirstName = "Updated", LastName = "Employee", Email = "updated@example.com", Phone = "123456789", Role = "Developer" };
            var handler = new SaveEmployeesCommandHandler(DbContext);

            var employee = new Employee { Id = 0, FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedList = await DbContext.Employees.SingleOrDefaultAsync(list => list.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedList);
            Assert.Equal(request.FirstName, savedList.FirstName);
        }

        [Fact]
        public async System.Threading.Tasks.Task Save_should_not_update_missing_list()
        {
            // Arrange
            var request = new SaveEmployeesCommand { Id = 20, FirstName = "Updated", LastName = "Employee", Email = "updated@example.com", Phone = "123456789", Role = "Developer" };
            var handler = new SaveEmployeesCommandHandler(DbContext);

            var employee = new Employee { Id = 0, FirstName = "Test", LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };
            await DbContext.Employees.AddAsync(employee);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890123456789012345678901234567890123456789000")]
        public void SaveValidator_should_return_false_when_title_is_invalid(string firstName)
        {
            // Arrange
            var validator = new SaveEmployeesCommandValidator(DbContext);
            var command = new SaveEmployeesCommand { Id = 0, FirstName = firstName, LastName = "Employee", Email = "test@example.com", Phone = "123456789", Role = "Developer" };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveEmployeesCommand.FirstName), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_title_is_valid()
        {
            // Arrange
            var validator = new SaveEmployeesCommandValidator(DbContext);
            var command = new SaveEmployeesCommand { Id = 0, FirstName = "Employee", LastName = "Test", Email = "employee@example.com", Phone = "123456789", Role = "Developer" };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_first_name()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 10, SearchFirstName = "John" };
            var handler = new ListEmployeesQueryHandler(DbContext);

            await DbContext.Employees.AddAsync(new Employee { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789", Role = "Developer" });
            await DbContext.Employees.AddAsync(new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Phone = "987654321", Role = "Manager" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("John", result.Value.Results.First().FirstName);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_last_name()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 10, SearchLastName = "Smith" };
            var handler = new ListEmployeesQueryHandler(DbContext);

            await DbContext.Employees.AddAsync(new Employee { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789", Role = "Developer" });
            await DbContext.Employees.AddAsync(new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Phone = "987654321", Role = "Manager" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Smith", result.Value.Results.First().LastName);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_search_by_email()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 10, SearchEmail = "jane" };
            var handler = new ListEmployeesQueryHandler(DbContext);

            await DbContext.Employees.AddAsync(new Employee { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789", Role = "Developer" });
            await DbContext.Employees.AddAsync(new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Phone = "987654321", Role = "Manager" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Contains("jane", result.Value.Results.First().Email);
        }

        [Fact]
        public async System.Threading.Tasks.Task List_should_return_empty_when_search_returns_no_results()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 10, SearchFirstName = "NonExistent" };
            var handler = new ListEmployeesQueryHandler(DbContext);

            await DbContext.Employees.AddAsync(new Employee { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789", Role = "Developer" });
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