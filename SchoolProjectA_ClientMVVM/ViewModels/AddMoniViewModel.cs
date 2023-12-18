using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels
{
    public class AddMoniViewModel : ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        private string _password;
        private string _passwordConfirmation;
        private string _login;

        public ViewModelBase ParentForm { get; set; }
        public AddMoniViewModel(ViewModelBase parentForm)
        {
            ParentForm = parentForm;
        }


        // Getters Setters
        public string FirstName 
        { 
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set => this.RaiseAndSetIfChanged(ref _passwordConfirmation, value);
        }

        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }
    }
}
