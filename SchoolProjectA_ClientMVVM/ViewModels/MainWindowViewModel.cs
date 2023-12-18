using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System.Windows.Input;

namespace SchoolProjectA_ClientMVVM.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    
    // The view model of either Global or Connexion view
    ViewModelBase _contentViewModel;
    // Enable status of the connection button
    private bool _isButtonEnabled;
    

    public MainWindowViewModel()
    {
        _contentViewModel = new ConnexionViewModel();
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

    // Global ViewModel
    public async void Connect()
    {
        IsButtonEnabled = false;
        // Fake connection
        Moni moni = await Queries.GetMoni("wan34");
        if (moni != null) 
        { 
            ContentViewModel = new GlobalViewModel(moni);
        }
        IsButtonEnabled = true;
    }

    // Connexion ViewModel
    public void Disconnect()
    {
        ContentViewModel = new ConnexionViewModel();
    }

    // Add Moni ViewModel
    public void CreateMoniForm()
    {
        ContentViewModel = new AddMoniViewModel(this);
    }
}

