using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace KooliProjekt.WpfApplication.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IDialogProvider> _dialogProviderMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _dialogProviderMock = new Mock<IDialogProvider>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object, _dialogProviderMock.Object);
        }

        [Fact]
        public void SelectedItem_should_return_correct_item()
        {
            // Arrange
            var item = new Employee { Id = 1, FirstName = "Test" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Equal(item, _viewModel.SelectedItem);
        }

        [Fact]
        public void SelectedItem_should_call_notify_property_changed()
        {
            // Arrange
            var item = new Employee { Id = 1, FirstName = "Test" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public async Task LoadCommand_should_load_data_from_api_client()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Employee>>
            {
                Value = new PagedResult<Employee>
                {
                    Results = new List<Employee>
                    {
                        new Employee { Id = 1, FirstName = "Test 1" },
                        new Employee { Id = 2, FirstName = "Test 2" }
                    }
                }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult);

            // Act            
            _viewModel.LoadCommand.Execute(null);
            
            // Wait for async command to complete (since we don't have a task to wait on, we might need a small delay or better sync)
            // In a real scenario, we might want to expose a Task from the command or use a different testing approach.
            // For now, let's use a small delay or just verify the behavior.
            await Task.Delay(100);

            // Assert
            _apiClientMock.Verify(client => client.List(1, 100), Times.AtLeastOnce);
            Assert.Equal(2, _viewModel.Data.Count);
        }

        [Fact]
        public async Task LoadCommand_should_show_error_when_api_client_fails()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Employee>>
            {
                Errors = new List<string> { "Error" }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult);

            // Act            
            _viewModel.LoadCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            _dialogProviderMock.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Viga laadimisel"))), Times.Once);
        }

        [Fact]
        public void NewCommand_Should_Set_Empty_SelectedItem()
        {
            // Act
            _viewModel.NewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
            Assert.Null(_viewModel.SelectedItem.FirstName);
        }

        [Fact]
        public async Task SaveCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var employee = new Employee { Id = 1, FirstName = "Test" };
            _viewModel.SelectedItem = employee;

            var saveDataApiResult = new OperationResult();
            var loadDataApiResult = new OperationResult<PagedResult<Employee>>
            {
                Value = new PagedResult<Employee> { Results = new List<Employee>() }
            };

            _apiClientMock.Setup(client => client.Save(employee))
                .ReturnsAsync(saveDataApiResult);
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult);

            // Act
            _viewModel.SaveCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            _apiClientMock.Verify(client => client.Save(employee), Times.Once);
            _apiClientMock.Verify(client => client.List(1, 100), Times.AtLeastOnce);
        }

        [Fact]
        public async Task SaveCommand_should_show_error_when_api_gave_error()
        {
            // Arrange
            var employee = new Employee { Id = 1, FirstName = "Test" };
            _viewModel.SelectedItem = employee;

            var saveDataApiResult = new OperationResult();
            saveDataApiResult.AddError("Error");

            _apiClientMock.Setup(client => client.Save(employee))
                .ReturnsAsync(saveDataApiResult);

            // Act
            _viewModel.SaveCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            _dialogProviderMock.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Viga salvestamisel"))), Times.Once);
            _apiClientMock.Verify(client => client.List(1, 100), Times.Never);
        }

        [Fact]
        public void SaveCommand_can_execute_when_selected_item_is_not_null()
        {
            // Act & Assert
            _viewModel.SelectedItem = null;
            Assert.False(_viewModel.SaveCommand.CanExecute(null));

            _viewModel.SelectedItem = new Employee();
            Assert.True(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public async Task DeleteCommand_should_return_when_no_confirmation()
        {
            // Arrange
            var employee = new Employee { Id = 1, FirstName = "Test" };
            _viewModel.SelectedItem = employee;
            _dialogProviderMock.Setup(x => x.Confirm(It.IsAny<string>())).Returns(false);

            // Act
            _viewModel.DeleteCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var employee = new Employee { Id = 1, FirstName = "Test" };
            _viewModel.SelectedItem = employee;
            _dialogProviderMock.Setup(x => x.Confirm(It.IsAny<string>())).Returns(true);

            var deleteResult = new OperationResult();
            var loadResult = new OperationResult<PagedResult<Employee>>
            {
                Value = new PagedResult<Employee> { Results = new List<Employee>() }
            };

            _apiClientMock.Setup(client => client.Delete(1)).ReturnsAsync(deleteResult);
            _apiClientMock.Setup(client => client.List(1, 100)).ReturnsAsync(loadResult);

            // Act
            _viewModel.DeleteCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            _apiClientMock.Verify(client => client.Delete(1), Times.Once);
            _apiClientMock.Verify(client => client.List(1, 100), Times.AtLeastOnce);
        }

        [Fact]
        public async Task DeleteCommand_should_show_error_when_api_gave_error()
        {
            // Arrange
            var employee = new Employee { Id = 1, FirstName = "Test" };
            _viewModel.SelectedItem = employee;
            _dialogProviderMock.Setup(x => x.Confirm(It.IsAny<string>())).Returns(true);

            var deleteResult = new OperationResult();
            deleteResult.AddError("Error");

            _apiClientMock.Setup(client => client.Delete(1)).ReturnsAsync(deleteResult);

            // Act
            _viewModel.DeleteCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            _dialogProviderMock.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Viga kustutamisel"))), Times.Once);
            _apiClientMock.Verify(client => client.List(1, 100), Times.Never);
        }

        [Fact]
        public void DeleteCommand_can_execute_when_selected_item_is_not_null()
        {
            // Act & Assert
            _viewModel.SelectedItem = null;
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));

            _viewModel.SelectedItem = new Employee { Id = 1 };
            Assert.True(_viewModel.DeleteCommand.CanExecute(null));
        }
    }
}
