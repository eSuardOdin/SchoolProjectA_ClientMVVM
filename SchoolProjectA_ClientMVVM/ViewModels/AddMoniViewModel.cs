using ReactiveUI;
using Avalonia.Media;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SchoolProjectA_ClientMVVM.Models;
namespace SchoolProjectA_ClientMVVM.ViewModels
{
    public class AddMoniViewModel : ViewModelBase
    {
        private string _firstName;
        private bool _isFirstNameValid;

        private string _lastName;
        private bool _isLastNameValid;

        private string _password;
        private string _passwordConfirmation;
        private bool _isPasswordValid;
            
        private string _login;
        private string _loginValidity;
        private bool _isLoginValid;

        private bool _createEnabled;

        public ViewModelBase ParentForm { get; set; }
        public AddMoniViewModel(ViewModelBase parentForm)
        {
            ParentForm = parentForm;
            this.WhenAnyValue(x => x.Login).Subscribe(_ => CheckLogin());
            this.WhenAnyValue(x => x.FirstName).Subscribe(_ => CheckFirstName());
            this.WhenAnyValue(x => x.LastName).Subscribe(_ => CheckLastName());
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

        public string LoginValidity
        {
            get => _loginValidity;
            set => this.RaiseAndSetIfChanged(ref _loginValidity, value);
        }

        public bool CreateEnabled
        {
            get => _createEnabled;
            set
            {
                _createEnabled = (/*_isPasswordValid && */_isLoginValid && _isFirstNameValid && _isLastNameValid);
                this.RaisePropertyChanged(nameof(CreateEnabled));
            }
        }
        /*
         * 
         * COMMANDS
         * 
         */
        /// <summary>
        /// Checks if login already exists
        /// </summary>
        /// <returns></returns>
        private async Task CheckLogin()
        {
            if (String.IsNullOrWhiteSpace(Login))
            {
                LoginValidity = "";
                _isLoginValid = false;
                this.RaisePropertyChanged(nameof(CreateEnabled));
            }
            else
            {
                Moni moni = await Queries.GetMoni(Login);
                if (moni == null)
                {
                    LoginValidity = $"Le pseudo {Login} est disponible";
                    _isLoginValid = true;
                    this.RaisePropertyChanged(nameof(CreateEnabled));
                }
                else
                {
                    LoginValidity = $"Le pseudo {Login} n'est pas disponible";
                    _isLoginValid = false;
                    this.RaisePropertyChanged(nameof(CreateEnabled));
                }
            }
        }

        private void CheckFirstName()
        {
            if(String.IsNullOrWhiteSpace(FirstName))
            {
                _isFirstNameValid = false;
                this.RaisePropertyChanged(nameof(CreateEnabled));
            }
            else
            {
                _isFirstNameValid = true;
                this.RaisePropertyChanged(nameof(CreateEnabled));
            }
        }

        private void CheckLastName()
        {
            if (String.IsNullOrWhiteSpace(LastName))
            {
                _isLastNameValid = false;
                this.RaisePropertyChanged(nameof(CreateEnabled));
            }
            else
            {
                _isLastNameValid = true;
                this.RaisePropertyChanged(nameof(CreateEnabled));
            }
        }
    }


}


/*
 * 
 * 
 *  public bool IsFirstNameValid
    {
        get => _isFirstNameValid;
        set
        {
            _isFirstNameValid = value;
            UpdateCreateEnabled();
        }
    }

    public bool IsLastNameValid
    {
        get => _isLastNameValid;
        set
        {
            _isLastNameValid = value;
            UpdateCreateEnabled();
        }
    }

    public bool CreateEnabled
    {
        get => _createEnabled;
        set => this.RaiseAndSetIfChanged(ref _createEnabled, value);
    }

    private void UpdateCreateEnabled()
    {
        CreateEnabled = IsLoginValid && IsFirstNameValid && IsLastNameValid;
    }
 * 
 * 
 * 
 */
