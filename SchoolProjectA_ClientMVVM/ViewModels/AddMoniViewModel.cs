using ReactiveUI;
using Avalonia.Media;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SchoolProjectA_ClientMVVM.Models;
using System.Text.RegularExpressions;
using System.Reactive;
using System.Diagnostics;
namespace SchoolProjectA_ClientMVVM.ViewModels
{
    public class AddMoniViewModel : ViewModelBase
    {
        // ----- Views -----
        private string _firstName;
        private string _firstNameValidity;
        private bool _isFirstNameValid;

        private string _lastName;
        private string _lastNameValidity;
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
        // -----------------


        // ----- Commands -----
        //public ReactiveCommand<Unit, Moni> AddCommand { get; }
        //public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        // -------------------

        public ViewModelBase ParentForm { get; set; }


        public AddMoniViewModel(ViewModelBase parentForm)
        {
            ParentForm = parentForm;
            this.WhenAnyValue(x => x.Login).Subscribe(_ => CheckLogin());
            this.WhenAnyValue(x => x.FirstName).Subscribe(_ => CheckFirstName());
            this.WhenAnyValue(x => x.LastName).Subscribe(_ => CheckLastName());
            this.WhenAnyValue(x => x.Password).Subscribe(_ => CheckPassword());
            this.WhenAnyValue(x => x.PasswordConfirmation).Subscribe(_ => CheckPassword());
        }


        /*
         * O-----------------O
         * | Getters Setters |
         * O-----------------O
         */

        // ------ Firstname ----
        public string FirstName 
        { 
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        public string FirstNameValidity
        {
            get => _firstNameValidity;
            set => this.RaiseAndSetIfChanged(ref _firstNameValidity, value);
        }
        // -------------------



        // ---- Lastname -----
        public string LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }

        public string LastNameValidity
        {
            get => _lastNameValidity;
            set => this.RaiseAndSetIfChanged(ref _lastNameValidity, value);
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

        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set => this.RaiseAndSetIfChanged(ref _isPasswordValid, value);
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
         * O----------O
         * | Commands |
         * O----------O
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
            }
            else
            {
                Moni moni = await Queries.GetMoni(Login);
                if (moni == null)
                {
                    LoginValidity = $"Le pseudo {Login} est disponible";
                    _isLoginValid = true;
                    LoginValidityColor = Brushes.Green;
                }
                else
                {
                    LoginValidity = $"Le pseudo {Login} n'est pas disponible";
                    _isLoginValid = false;
                    LoginValidityColor = Brushes.Red;
                }
            }
            UpdateCreateEnabled();
        }

        /// <summary>
        /// Checks if firstname ok
        /// </summary>
        private void CheckFirstName()
        {
            if(String.IsNullOrWhiteSpace(FirstName))
            {
                _isFirstNameValid = false;
                FirstNameValidity = "Le champ prénom doit être rempli";
            }
            else
            {
                if (Regex.IsMatch(FirstName, @"^[a-zA-Z]+$"))
                {
                    _isFirstNameValid = true;
                    FirstNameValidity = "";
                }
                else
                {
                    _isFirstNameValid = false;
                    FirstNameValidity = "Le prénom ne doit contenir que des lettres";
                }
            }
            UpdateCreateEnabled();
        }

        /// <summary>
        /// Checks if lastname ok
        /// </summary>
        private void CheckLastName()
        {
            if (String.IsNullOrWhiteSpace(LastName))
            {
                _isLastNameValid = false;
                LastNameValidity = "Le champ nom doit être rempli";
            }
            else
            {
                if (Regex.IsMatch(LastName, @"^[a-zA-Z]+$"))
                {
                    _isLastNameValid = true;
                    LastNameValidity = "";
                }
                else
                {
                    _isLastNameValid = false;
                    LastNameValidity = "Le nom ne doit contenir que des lettres";
                }
            }
            UpdateCreateEnabled();
        }

        /// <summary>
        /// Checks password + password confirmation validity
        /// </summary>
        private void CheckPassword()
        {
            if(String.IsNullOrWhiteSpace(_password) || String.IsNullOrEmpty(_passwordConfirmation))
            {
                IsPasswordValid = false;
                PasswordValidity = "Les champs de mot de passe doivent être remplis";
            }
            else if(_password != _passwordConfirmation)
            {
                IsPasswordValid = false;
                PasswordValidity = "Les champs de mot de passe doivent correspondre";
            }
            else
            {
                IsPasswordValid = true;
                PasswordValidity = "";
            }
            UpdateCreateEnabled();
        }


        /// <summary>
        /// Updates the validation button enabled status
        /// </summary>
        private void UpdateCreateEnabled()
        {
            CreateEnabled = _isPasswordValid && _isLoginValid && _isFirstNameValid && _isLastNameValid;
        }


        public async Task AddMoni()
        {
            CreateEnabled = false;
            Moni moni = new Moni
            {
                FirstName = FirstName,
                LastName = LastName,
                MoniPwd = Password,
                MoniLogin = Login
            };

            Moni postedMoni = await Queries.PostMoni(moni);

            if (postedMoni != null)
            {
                System.Diagnostics.Debug.WriteLine($"{postedMoni.MoniLogin} ajouté avec id {postedMoni.MoniId}");
                Login = "";
                Password = "";
                PasswordConfirmation = "";
                LastName = "";
                FirstName = "";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Erreur d'insertion dans la base");

            }
        }
    }


}

