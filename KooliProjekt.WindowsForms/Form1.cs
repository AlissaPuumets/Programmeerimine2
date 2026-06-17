using System.ComponentModel;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form, IMainView
    {
        private readonly IApiClient _apiClient;
        private MainViewPresenter _mainViewPresenter;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<Employee> DataSource
        {
            get { return (IList<Employee>)dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Employee SelectedItem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentId
        {
            get { return int.Parse(idField.Text); }
            set { idField.Text = value.ToString(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentFirstName
        {
            get { return firstNameField.Text; }
            set { firstNameField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentLastName
        {
            get { return lastNameField.Text; }
            set { lastNameField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentEmail
        {
            get { return emailField.Text; }
            set { emailField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentPhone
        {
            get { return phoneField.Text; }
            set { phoneField.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentRole
        {
            get { return roleField.Text; }
            set { roleField.Text = value; }
        }

        public void SetPresenter(MainViewPresenter presenter)
        {
            _mainViewPresenter = presenter;
        }

        public Form1(IApiClient apiClient)
        {
            _apiClient = apiClient;

            InitializeComponent();

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            saveCommand.Click += SaveCommand_Click;
            addCommand.Click += AddCommand_Click;
            deleteCommand.Click += DeleteCommand_Click;
        }

        private async void DeleteCommand_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Delete();
        }

        private void AddCommand_Click(object sender, EventArgs e)
        {
            _mainViewPresenter.SetSelection(null);
        }

        private async void SaveCommand_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Save();
        }

        public bool ConfirmDelete()
        {
            var message = "Oled kindel, et soovid kustutada " + CurrentFirstName + " " + CurrentLastName + "?";
            var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (answer == DialogResult.Yes);
        }

        // Koosta etteantud veateatest ja OperationResult sees olevatest vigadest
        // veateade ja näita seda kasutajale
        public void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = "";
            var propertyErrors = "";

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            error = error.Trim();

            MessageBox.Show(error, "Viga!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                _mainViewPresenter.SetSelection(null);
                return;
            }

            var selectedList = (Employee)dataGridView1.CurrentRow.DataBoundItem;
            _mainViewPresenter.SetSelection(selectedList);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await _mainViewPresenter.LoadData();
        }
    }
}