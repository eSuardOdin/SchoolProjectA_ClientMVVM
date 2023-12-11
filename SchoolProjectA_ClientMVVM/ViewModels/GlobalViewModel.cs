using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;
public class GlobalViewModel : ViewModelBase
{
    private ViewModelBase _containedViewModel;
    public GlobalViewModel()
    {
        _containedViewModel = new AccountsViewModel();
    }

    public ViewModelBase ContainedViewModel
    {
        get => _containedViewModel;
        set => this.RaiseAndSetIfChanged(ref _containedViewModel, value);
    }

    public void ShowTransactions()
    {
        System.Diagnostics.Debug.WriteLine("Clic sur transaction");
        ContainedViewModel = new TransactionsViewModel();
    }

    public void ShowAccounts()
    {
        System.Diagnostics.Debug.WriteLine("Clic sur account");
        ContainedViewModel = new AccountsViewModel();
    }

    public void ShowTags()
    {
        System.Diagnostics.Debug.WriteLine("Clic sur tag");
        ContainedViewModel = new TagsViewModel();
    }
}
