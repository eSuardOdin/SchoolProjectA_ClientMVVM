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
    //private ObservableCollection<BankAccount> _bankAccounts;
    private int MoniId {  get; set; }
    public ShowAccountsViewModel AccountsList { get; }
    public AccountsViewModel(int moniId)
    {
        MoniId = moniId;
        AccountsList = new(MoniId);
        _accountsContentViewModel = AccountsList;
        //InitializeAsync(MoniId);
    }

    /// <summary>
    /// Change of ViewModel
    /// </summary>
    public ViewModelBase AccountsContentViewModel
    {
        get => _accountsContentViewModel;
        set => this.RaiseAndSetIfChanged(ref this._accountsContentViewModel, value);
    }
    /*
    /// <summary>
    /// Change in Accounts list
    /// </summary>
    public ObservableCollection<BankAccount> BankAccounts
    {
        get => _bankAccounts;
        set => this.RaiseAndSetIfChanged(ref this._bankAccounts, value);
    }
    */

    /// <summary>
    /// Showing add account form
    /// </summary>
    public void AddAccount()
    {
        //AccountsContentViewModel = new AddAccountViewModel();
        AddAccountViewModel addAccountVM = new AddAccountViewModel();
        Observable.Merge(
            addAccountVM.AddCommand,
            addAccountVM.CancelCommand.Select(_ => (BankAccount?)null))
            .Take(1) // Getting only the first click
            .Subscribe(newBankAccount =>
            { if (newBankAccount != null)
                {
                    AccountsList.BankAccounts.Add(newBankAccount);
                }
                AccountsContentViewModel = AccountsList;
            });
        AccountsContentViewModel = addAccountVM;
    }


    // Remonté depuis ShowAccountVM
    /*
    // Load accounts
    private async Task<List<BankAccount>> LoadBankAccounts(int moniId)
    {
        List<BankAccount> bankAccounts = await Queries.GetMoniAccounts(moniId);
        foreach (BankAccount bankAccount in bankAccounts) { System.Diagnostics.Debug.WriteLine(bankAccount.BankAccountLabel); }
        return bankAccounts;
    }
    // Casting list to ObservableCollection
    private async void InitializeAsync(int moniId)
    {
        List<BankAccount> accounts = await LoadBankAccounts(moniId);
        BankAccounts = new ObservableCollection<BankAccount>(accounts);
    }
    */


}
