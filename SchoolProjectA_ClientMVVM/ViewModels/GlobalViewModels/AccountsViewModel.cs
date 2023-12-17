using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class AccountsViewModel : ViewModelBase
{
    ViewModelBase _accountsContentViewModel;
    private int MoniId {  get; set; }
    public ShowAccountsViewModel AccountsList { get; }
    public AccountsViewModel(int moniId)
    {
        MoniId = moniId;
        AccountsList = new(MoniId);
        _accountsContentViewModel = AccountsList;
    }

    /// <summary>
    /// Change of ViewModel
    /// </summary>
    public ViewModelBase AccountsContentViewModel
    {
        get => _accountsContentViewModel;
        set => this.RaiseAndSetIfChanged(ref this._accountsContentViewModel, value);
    }

    /// <summary>
    /// Showing add account form
    /// </summary>
    public async void AddAccount()
    {
        AddAccountViewModel addAccountVM = new AddAccountViewModel();
        Observable.Merge(
            addAccountVM.AddCommand,
            addAccountVM.CancelCommand.Select(_ => (BankAccount?)null))
            .Take(1) // Getting only the first click
            .Subscribe(async newBankAccount =>
            { if (newBankAccount != null)
                {
                    newBankAccount.MoniId = MoniId;
                    newBankAccount = await Queries.PostBankAccount(newBankAccount);
                    if(newBankAccount != null)
                    {
                        AccountsList.BankAccounts.Add(newBankAccount);
                    }
                }
                AccountsContentViewModel = AccountsList;
            });
        AccountsContentViewModel = addAccountVM;
    }


}
