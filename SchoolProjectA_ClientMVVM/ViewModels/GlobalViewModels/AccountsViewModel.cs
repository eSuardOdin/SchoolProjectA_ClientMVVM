using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class AccountsViewModel : ViewModelBase
{
    ViewModelBase _accountsContentViewModel;
    
    public AccountsViewModel(int moniId)
    {
        
        _accountsContentViewModel = new ShowAccountsViewModel(moniId);
    }

    // Change of ViewModel
    public ViewModelBase AccountsContentViewModel
    {
        get => _accountsContentViewModel;
        set => this.RaiseAndSetIfChanged(ref this._accountsContentViewModel, value);
    }

    
    
}
