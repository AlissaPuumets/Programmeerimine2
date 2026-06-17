using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<Employee> DataSource { get; set; }
        Employee SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentFirstName { get; set; }
        string CurrentLastName { get; set; }
        string CurrentEmail { get; set; }
        string CurrentPhone { get; set; }
        string CurrentRole { get; set; }
        bool ConfirmDelete();
    }
}
