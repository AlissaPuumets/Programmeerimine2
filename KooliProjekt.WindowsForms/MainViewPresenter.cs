using System.Threading.Tasks;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class MainViewPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IMainView _mainView;

        private Employee _selectedList;

        public MainViewPresenter(IApiClient apiClient, IMainView mainView)
        {
            _apiClient = apiClient;
            _mainView = mainView;
            _mainView.SetPresenter(this);
        }

        public async Task LoadData()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                _mainView.ShowError("Viga andmete laadimisel", response);
                _mainView.DataSource = null;
                return;
            }

            _mainView.DataSource = response.Value.Results;
        }

        public void SetSelection(Employee selectedList)
        {
            _selectedList = selectedList;
            if (_selectedList == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentFirstName = "";
                _mainView.CurrentLastName = "";
                _mainView.CurrentEmail = "";
                _mainView.CurrentPhone = "";
                _mainView.CurrentRole = "";
            }
            else
            {
                _mainView.CurrentId = _selectedList.Id;
                _mainView.CurrentFirstName = _selectedList.FirstName;
                _mainView.CurrentLastName = _selectedList.LastName;
                _mainView.CurrentEmail = _selectedList.Email;
                _mainView.CurrentPhone = _selectedList.Phone;
                _mainView.CurrentRole = _selectedList.Role;
            }
        }

        public async Task Save()
        {
            var employee = new Employee();
            employee.Id = _mainView.CurrentId;
            employee.FirstName = _mainView.CurrentFirstName;
            employee.LastName = _mainView.CurrentLastName;
            employee.Email = _mainView.CurrentEmail;
            employee.Phone = _mainView.CurrentPhone;
            employee.Role = _mainView.CurrentRole;

            var result = await _apiClient.Save(employee);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga salvestamisel", result);
                return;
            }

            await LoadData();
        }

        public async Task Delete()
        {
            if (!_mainView.ConfirmDelete())
            {
                return;
            }

            var result = await _apiClient.Delete(_mainView.CurrentId);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga kustutamisel", result);
                return;
            }

            await LoadData();
        }
    }
}
