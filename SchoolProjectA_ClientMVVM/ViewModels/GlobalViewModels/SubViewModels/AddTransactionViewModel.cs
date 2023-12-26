using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using SchoolProjectA_ClientMVVM.Models;
using System.Collections.ObjectModel;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class AddTransactionViewModel : ViewModelBase
{
    private string _transactionLabel;
    private string? _transactionDescription;
    private decimal _transactionAmount;
    private DateTime _transactionDate;
    //private int _bankAccountId;
    private int[] _tags;
    private ObservableCollection<BankAccount>? _bankAccounts;

    public string TransactionLabel
    {
        get => _transactionLabel;
        set => this.RaiseAndSetIfChanged(ref _transactionLabel, value);
    }

    public string TransactionDescription
    {
        get => _transactionDescription;
        set => this.RaiseAndSetIfChanged(ref _transactionDescription, value);
    }

    public decimal TransactionAmount
    {
        get => _transactionAmount;
        set => this.RaiseAndSetIfChanged(ref _transactionAmount, value);
    }

    public DateTime TransactionDate
    {
        get => _transactionDate; 
        set => this.RaiseAndSetIfChanged(ref _transactionDate, value);
    }

    public int[] Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }

    public ObservableCollection<BankAccount> BankAccounts
    {
        get => _bankAccounts; 
        set => this.RaiseAndSetIfChanged(ref _bankAccounts, value);
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

    public int MoniId {  get; set; }

    public ReactiveCommand<Unit, Transaction> AddTransactionCommand { get; set; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

    public AddTransactionViewModel(int moniId)
    {
        MoniId = moniId;
        InitializeAsync(MoniId);
    }


}
