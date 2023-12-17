using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class ShowAccountsViewModel : ViewModelBase
{
    private ObservableCollection<BankAccount> _bankAccounts;
    private int MoniId;

    public ObservableCollection<BankAccount> BankAccounts
    {
        get => _bankAccounts;
        set => this.RaiseAndSetIfChanged(ref this._bankAccounts, value);
    }

    public ShowAccountsViewModel(int moniId)
    {
        MoniId = moniId;
        InitializeAsync(MoniId);
    }

    
    // Load accounts
    private async Task<List<BankAccount>> LoadBankAccounts(int moniId)
    {
        List<BankAccount> bankAccounts = await Queries.GetMoniAccounts(moniId);
        foreach(BankAccount bankAccount in bankAccounts) { System.Diagnostics.Debug.WriteLine(bankAccount.BankAccountLabel); }
        return bankAccounts;
    }
    // Casting list to ObservableCollection
    private async void InitializeAsync(int moniId)
    {
        List<BankAccount> accounts = await LoadBankAccounts(moniId);
        BankAccounts = new ObservableCollection<BankAccount>(accounts);
    }


    // Handling color to do

}

