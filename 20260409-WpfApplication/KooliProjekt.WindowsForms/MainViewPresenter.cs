using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class MainViewPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IMainView _mainView;

        private ToDoList _selectedList;

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

        public void SetSelection(ToDoList selectedList)
        {
            _selectedList = selectedList;
            if (_selectedList == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentTitle = "";
            }
            else
            {
                _mainView.CurrentId = _selectedList.Id;
                _mainView.CurrentTitle = _selectedList.Title;
            }
        }

        public async Task Save()
        {
            var todoList = new ToDoList();
            todoList.Id = _mainView.CurrentId;
            todoList.Title = _mainView.CurrentTitle;

            var result = await _apiClient.Save(todoList);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga salvestamisel", result);
                return;
            }

            await LoadData();
        }

        public async Task Delete()
        {
            if(!_mainView.ConfirmDelete())
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
