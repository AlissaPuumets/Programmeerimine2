using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class MainViewPresenterTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMainView> _mainViewMock;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _mainViewMock = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClientMock.Object, _mainViewMock.Object);
        }

        [Fact]
        public async Task LoadData_should_set_datasource_on_success()
        {
            // Arrange
            var employees = new List<Employee> { new Employee { Id = 1, FirstName = "John" } };
            var pagedResult = new PagedResult<Employee> { Results = employees };
            var operationResult = new OperationResult<PagedResult<Employee>>(pagedResult);

            _apiClientMock.Setup(x => x.List(1, 100)).ReturnsAsync(operationResult);

            // Act
            await _presenter.LoadData();

            // Assert
            _mainViewMock.VerifySet(x => x.DataSource = employees);
        }

        [Fact]
        public async Task LoadData_should_show_error_on_failure()
        {
            // Arrange
            var operationResult = new OperationResult<PagedResult<Employee>>();
            operationResult.AddError("Error");

            _apiClientMock.Setup(x => x.List(1, 100)).ReturnsAsync(operationResult);

            // Act
            await _presenter.LoadData();

            // Assert
            _mainViewMock.Verify(x => x.ShowError("Viga andmete laadimisel", operationResult));
            _mainViewMock.VerifySet(x => x.DataSource = null);
        }

        [Fact]
        public void SetSelection_should_set_view_properties_when_employee_is_not_null()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Phone = "123",
                Role = "Admin"
            };

            // Act
            _presenter.SetSelection(employee);

            // Assert
            _mainViewMock.VerifySet(x => x.CurrentId = 1);
            _mainViewMock.VerifySet(x => x.CurrentFirstName = "John");
            _mainViewMock.VerifySet(x => x.CurrentLastName = "Doe");
            _mainViewMock.VerifySet(x => x.CurrentEmail = "john@doe.com");
            _mainViewMock.VerifySet(x => x.CurrentPhone = "123");
            _mainViewMock.VerifySet(x => x.CurrentRole = "Admin");
        }

        [Fact]
        public void SetSelection_should_clear_view_properties_when_employee_is_null()
        {
            // Act
            _presenter.SetSelection(null);

            // Assert
            _mainViewMock.VerifySet(x => x.CurrentId = 0);
            _mainViewMock.VerifySet(x => x.CurrentFirstName = "");
            _mainViewMock.VerifySet(x => x.CurrentLastName = "");
            _mainViewMock.VerifySet(x => x.CurrentEmail = "");
            _mainViewMock.VerifySet(x => x.CurrentPhone = "");
            _mainViewMock.VerifySet(x => x.CurrentRole = "");
        }

        [Fact]
        public async Task Save_should_call_api_and_reload_data_on_success()
        {
            // Arrange
            _mainViewMock.SetupGet(x => x.CurrentId).Returns(1);
            _mainViewMock.SetupGet(x => x.CurrentFirstName).Returns("John");
            _mainViewMock.SetupGet(x => x.CurrentLastName).Returns("Doe");
            _mainViewMock.SetupGet(x => x.CurrentEmail).Returns("john@doe.com");
            _mainViewMock.SetupGet(x => x.CurrentPhone).Returns("123");
            _mainViewMock.SetupGet(x => x.CurrentRole).Returns("Admin");

            var operationResult = new OperationResult();
            _apiClientMock.Setup(x => x.Save(It.IsAny<Employee>())).ReturnsAsync(operationResult);

            // Mocking LoadData behavior
            var pagedResult = new PagedResult<Employee> { Results = new List<Employee>() };
            _apiClientMock.Setup(x => x.List(1, 100)).ReturnsAsync(new OperationResult<PagedResult<Employee>>(pagedResult));

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.Verify(x => x.Save(It.Is<Employee>(e => 
                e.Id == 1 && 
                e.FirstName == "John" && 
                e.LastName == "Doe" && 
                e.Email == "john@doe.com" && 
                e.Phone == "123" && 
                e.Role == "Admin")), Times.Once);
            _apiClientMock.Verify(x => x.List(1, 100), Times.Once);
        }

        [Fact]
        public async Task Save_should_show_error_on_failure()
        {
            // Arrange
            var operationResult = new OperationResult();
            operationResult.AddError("Error");
            _apiClientMock.Setup(x => x.Save(It.IsAny<Employee>())).ReturnsAsync(operationResult);

            // Act
            await _presenter.Save();

            // Assert
            _mainViewMock.Verify(x => x.ShowError("Viga salvestamisel", operationResult));
            _apiClientMock.Verify(x => x.List(1, 100), Times.Never);
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_not_confirmed()
        {
            // Arrange
            _mainViewMock.Setup(x => x.ConfirmDelete()).Returns(false);

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_api_and_reload_data_on_success()
        {
            // Arrange
            _mainViewMock.Setup(x => x.ConfirmDelete()).Returns(true);
            _mainViewMock.SetupGet(x => x.CurrentId).Returns(1);

            var operationResult = new OperationResult();
            _apiClientMock.Setup(x => x.Delete(1)).ReturnsAsync(operationResult);

            // Mocking LoadData behavior
            var pagedResult = new PagedResult<Employee> { Results = new List<Employee>() };
            _apiClientMock.Setup(x => x.List(1, 100)).ReturnsAsync(new OperationResult<PagedResult<Employee>>(pagedResult));

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.Verify(x => x.Delete(1), Times.Once);
            _apiClientMock.Verify(x => x.List(1, 100), Times.Once);
        }

        [Fact]
        public async Task Delete_should_show_error_on_failure()
        {
            // Arrange
            _mainViewMock.Setup(x => x.ConfirmDelete()).Returns(true);
            _mainViewMock.SetupGet(x => x.CurrentId).Returns(1);

            var operationResult = new OperationResult();
            operationResult.AddError("Error");
            _apiClientMock.Setup(x => x.Delete(1)).ReturnsAsync(operationResult);

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.Verify(x => x.ShowError("Viga kustutamisel", operationResult));
            _apiClientMock.Verify(x => x.List(1, 100), Times.Never);
        }

        // See mõelge ise välja: Test selection clearing when LoadData fails
        [Fact]
        public async Task LoadData_should_clear_datasource_when_has_errors()
        {
            // Arrange
            var operationResult = new OperationResult<PagedResult<Employee>>();
            operationResult.AddError("Error");
            _apiClientMock.Setup(x => x.List(1, 100)).ReturnsAsync(operationResult);

            // Act
            await _presenter.LoadData();

            // Assert
            _mainViewMock.VerifySet(x => x.DataSource = null);
        }
    }
}
