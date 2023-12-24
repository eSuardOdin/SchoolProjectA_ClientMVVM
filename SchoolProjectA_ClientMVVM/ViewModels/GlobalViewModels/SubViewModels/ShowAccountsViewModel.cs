using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class ShowAccountsViewModel : ViewModelBase
{
    private ObservableCollection<BankAccount> _bankAccounts;
    private int MoniId;
    public ReactiveCommand<int, Unit> DeleteBankAccountCommand { get; }
    public ObservableCollection<BankAccount> BankAccounts
    {
        get => _bankAccounts;
        set => this.RaiseAndSetIfChanged(ref this._bankAccounts, value);
    }

    public ShowAccountsViewModel(int moniId)
    {
        MoniId = moniId;
        InitializeAsync(MoniId);
        DeleteBankAccountCommand = ReactiveCommand.Create<int>(DeleteAccount);
    }

    
    // Load accounts
    private async Task<List<BankAccount>> LoadBankAccounts(int moniId)
    {
        List<BankAccount> bankAccounts = await Queries.GetMoniAccounts(moniId);
        return bankAccounts;
    }
    // Casting list to ObservableCollection
    private async void InitializeAsync(int moniId)
    {
        List<BankAccount> accounts = await LoadBankAccounts(moniId);
        BankAccounts = new ObservableCollection<BankAccount>(accounts);
    }


    private async void DeleteAccount(int accountId)
    {
        string res = await Queries.DeleteAccount(accountId);
        if (res != null)
        {
            BankAccounts.Remove(BankAccounts.FirstOrDefault(item => item.BankAccountId == accountId));
        }
    }
    // Handling color to do

}

