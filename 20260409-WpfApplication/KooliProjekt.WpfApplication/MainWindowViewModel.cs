using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private readonly IDialogProvider _dialogProvider;
        private ObservableCollection<Employee> _data;
        private Employee _selectedItem;

        public ObservableCollection<Employee> Data 
        { 
            get => _data;
            set
            {
                _data = value;
                NotifyPropertyChanged();
            }
        }

        public Employee SelectedItem 
        { 
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand LoadCommand { get; }

        public MainWindowViewModel() : this(new ApiClient(), new DialogProvider())
        {
        }

        public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogProvider)
        {
            _apiClient = apiClient;
            _dialogProvider = dialogProvider;
            _data = new ObservableCollection<Employee>();

            NewCommand = new RelayCommand<object>(OnNew);
            SaveCommand = new RelayCommand<object>(async (p) => await OnSave(), CanSaveOrDelete);
            DeleteCommand = new RelayCommand<object>(async (p) => await OnDelete(), CanSaveOrDelete);
            LoadCommand = new RelayCommand<object>(async (p) => await OnLoad());
        }

        private void OnNew(object parameter)
        {
            SelectedItem = new Employee();
        }

        private bool CanSaveOrDelete(object parameter)
        {
            return SelectedItem != null;
        }

        private async Task OnSave()
        {
            if (SelectedItem == null) return;

            var result = await _apiClient.Save(SelectedItem);
            if (result.HasErrors)
            {
                _dialogProvider.ShowError("Viga salvestamisel: " + string.Join(", ", result.Errors ?? new List<string>()));
                return;
            }

            await OnLoad();
        }

        private async Task OnDelete()
        {
            if (SelectedItem == null) return;

            if (!_dialogProvider.Confirm("Kas oled kindel, et soovid kustutada?"))
            {
                return;
            }

            var result = await _apiClient.Delete(SelectedItem.Id);
            if (result.HasErrors)
            {
                _dialogProvider.ShowError("Viga kustutamisel: " + string.Join(", ", result.Errors ?? new List<string>()));
                return;
            }

            await OnLoad();
        }

        private async Task OnLoad()
        {
            var result = await _apiClient.List(1, 100);
            if (result.HasErrors)
            {
                _dialogProvider.ShowError("Viga laadimisel: " + string.Join(", ", result.Errors ?? new List<string>()));
                return;
            }

            Action updateAction = () =>
            {
                Data = new ObservableCollection<Employee>(result.Value.Results);
                SelectedItem = null;
            };

            if (App.Current?.Dispatcher != null)
            {
                App.Current.Dispatcher.Invoke(updateAction);
            }
            else
            {
                updateAction();
            }
        }
    }
}
