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
        private string _passwordValidity;
        private bool _isPasswordValid;
            
        private string _login;
        private string _loginValidity;
        private IBrush _loginValidityColor = Brushes.Black;
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

        // ------ Firstname ----
        public string FirstName 
        { 
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }
        // -------------------



        // ---- Lastname -----
        public string LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }
        // -------------------



        // ------ Password -----
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
        public string PasswordValidity
        {
            get => _passwordValidity;
            set => this.RaiseAndSetIfChanged(ref _passwordValidity, value);
        }
        // -------------------


        // ------  Login  -----
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


        public IBrush LoginValidityColor
        {
            get => _loginValidityColor;
            set => this.RaiseAndSetIfChanged(ref _loginValidityColor, value);
        }
        // -------------------



        public bool CreateEnabled
        {
            get => _createEnabled;
            set => this.RaiseAndSetIfChanged(ref _createEnabled, value);
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
                LoginValidityColor = Brushes.Red;
                UpdateCreateEnabled();
            }
            else
            {
                Moni moni = await Queries.GetMoni(Login);
                if (moni == null)
                {
                    LoginValidity = $"Le pseudo {Login} est disponible";
                    _isLoginValid = true;
                    LoginValidityColor = Brushes.Green;
                    UpdateCreateEnabled();
                }
                else
                {
                    LoginValidity = $"Le pseudo {Login} n'est pas disponible";
                    _isLoginValid = false;
                    LoginValidityColor = Brushes.Red;
                    UpdateCreateEnabled();
                }
            }
        }

        /// <summary>
        /// Checks if firstname ok
        /// </summary>
        private void CheckFirstName()
        {
            if(String.IsNullOrWhiteSpace(FirstName))
            {
                _isFirstNameValid = false;
                UpdateCreateEnabled();
            }
            else
            {
                _isFirstNameValid = true;
                UpdateCreateEnabled();
            }
        }

        /// <summary>
        /// Checks if lastname ok
        /// </summary>
        private void CheckLastName()
        {
            if (String.IsNullOrWhiteSpace(LastName))
            {
                _isLastNameValid = false;
                UpdateCreateEnabled();
            }
            else
            {
                _isLastNameValid = true;
                UpdateCreateEnabled();
            }
        }






        private void UpdateCreateEnabled()
        {
            CreateEnabled = /*_isPasswordValid &&*/ _isLoginValid && _isFirstNameValid && _isLastNameValid;
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
