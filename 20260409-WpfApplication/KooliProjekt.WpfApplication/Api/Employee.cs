namespace KooliProjekt.WpfApplication
{
    public class Employee : NotifyPropertyChangedBase
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _role;

        public int Id
        {
            get => _id;
            set { _id = value; NotifyPropertyChanged(); }
        }

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; NotifyPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; NotifyPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; NotifyPropertyChanged(); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; NotifyPropertyChanged(); }
        }

        public string Role
        {
            get => _role;
            set { _role = value; NotifyPropertyChanged(); }
        }
    }
}
