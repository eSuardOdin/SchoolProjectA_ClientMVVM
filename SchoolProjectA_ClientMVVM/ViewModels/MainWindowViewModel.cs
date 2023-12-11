using ReactiveUI;

namespace SchoolProjectA_ClientMVVM.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    // The view model of either Global or Connexion view
    ViewModelBase _contentViewModel;

    public MainWindowViewModel()
    {
        /*
        // Will begin on connexion page
        _contentViewModel = new ConnexionViewModel();
        */
        _contentViewModel = new GlobalViewModel();
    }

    // Change of ViewModel
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    // Global ViewModel
    public void Connect()
    {
        ContentViewModel = new GlobalViewModel();
    }

    // Connexion ViewModel
    public void Disconnect()
    {
        ContentViewModel = new ConnexionViewModel();
    }
}

