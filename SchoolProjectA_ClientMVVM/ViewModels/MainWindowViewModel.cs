using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using Avalonia.Media;
using System.Drawing;
using System.Windows.Input;

namespace SchoolProjectA_ClientMVVM.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    
    // The view model of either Global or Connexion view
    ViewModelBase _contentViewModel;
    // Enable status of the connection button
    private bool _isButtonEnabled;
    // The connexion request result
    private string _connexionError;
    // Login and password
    private string _login;
    private string _password;

    public MainWindowViewModel()
    {
        _contentViewModel = new ConnexionViewModel();
        ConnexionError = "";
        IsButtonEnabled = true;
    }

    // Change of ViewModel
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    // Change of button status
    public bool IsButtonEnabled
    {
        get => _isButtonEnabled;
        set => this.RaiseAndSetIfChanged(ref this._isButtonEnabled, value);
    }

    // Get connexion error
    public string ConnexionError
    {
        get => _connexionError;
        set => this.RaiseAndSetIfChanged(ref this._connexionError, value);
    }
    // Password and login
    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref this._login, value);
    }
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref this._password, value);
    }

    // Global ViewModel
    public async void Connect()
    {
        ConnexionError = "";
        IsButtonEnabled = false;
        // Handle empty boxes
        if(string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Login))
        {
            ConnexionError = "Les champs de login et mot de passe doivent être remplis";
        }
        else
        {
            // Fake connection
            Moni moni = await Queries.GetMoni(Login);
            if (moni != null) 
            {
                Moni checkedMoni = await Queries.CheckMoni(moni, Password);
                if (checkedMoni != null)
                {
                    ContentViewModel = new GlobalViewModel(checkedMoni);
                }
                else
                {
                    ConnexionError = "Les champs de login et mot de passe ne correspondent pas";
                }
            }
            else
            {
                ConnexionError = "Les champs de login et mot de passe ne correspondent pas";
            }
        }
        IsButtonEnabled = true;
    }

    // Connexion ViewModel
    public void Disconnect(SolidColorBrush color, string status = "")
    {
        ContentViewModel = new ConnexionViewModel(color, status);
    }

    // Add Moni ViewModel
    public void CreateMoniForm()
    {
        ContentViewModel = new AddMoniViewModel(this);
    }
}

