namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<ToDoList> DataSource { get; set; }
        ToDoList SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentTitle { get; set; }
        bool ConfirmDelete();
    }
}
