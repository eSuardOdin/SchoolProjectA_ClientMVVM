using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;


namespace SchoolProjectA_ClientMVVM.ViewModels;

public class ShowTransactionsViewModel : ViewModelBase
{
    private ObservableCollection<Transaction> _transactions;
    private DateTimeOffset _transactionStartDate = DateTime.Today;
    private DateTimeOffset _transactionEndDate = DateTime.Today;
    private ObservableCollection<Tag>? _tags;
    private ObservableCollection<Tag>? _selectedTags = new();
    private ObservableCollection<BankAccount>? _bankAccounts;
    private BankAccount? _selectedBankAccount = null;
    private int MyMoniId { get; set; }

    /*
     * Attributes
     */
    public DateTimeOffset TransactionStartDate
    {
        get => _transactionStartDate;
        set => this.RaiseAndSetIfChanged(ref _transactionStartDate, value);
    }

    public DateTimeOffset TransactionEndDate
    {
        get => _transactionEndDate;
        set => this.RaiseAndSetIfChanged(ref _transactionEndDate, value);
    }

    public ObservableCollection<Tag> Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref this._tags, value);
    }
    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set => this.RaiseAndSetIfChanged(ref this._transactions, value);
    }

    public ObservableCollection<Tag>? SelectedTags
    {
        get => _selectedTags;
        set => this.RaiseAndSetIfChanged(ref _selectedTags, value);
    }

    public ObservableCollection<BankAccount> BankAccounts
    {
        get => _bankAccounts;
        set => this.RaiseAndSetIfChanged(ref _bankAccounts, value);
    }

    public BankAccount? SelectedBankAccount
    {
        get => _selectedBankAccount;
        set => this.RaiseAndSetIfChanged(ref _selectedBankAccount, value);
    }

    public ReactiveCommand<int, Unit> DeleteTransactionCommand { get; }
    public ReactiveCommand<Unit,Unit> RefreshTransactionCommand { get; }
    public ShowTransactionsViewModel(int moniId) 
    {
        MyMoniId = moniId;
        InitializeAsync();
        DeleteTransactionCommand = ReactiveCommand.Create<int>(DeleteTransaction);
    }

    // Load transactions
    private async Task<List<Transaction>> LoadTransactions()
    {
        List<BankAccount> bankAccounts = await Queries.GetMoniAccounts(MyMoniId);
        List<Transaction> transactions = new List<Transaction>();
        foreach (BankAccount bankAccount in bankAccounts)
        {
            List<Transaction> cachedTransac = await Queries.GetAccountTransactions(bankAccount.BankAccountId);
            foreach (Transaction transaction in cachedTransac)
            {
                transactions.Add(transaction);
            }
        }
        return transactions;
    }
    //Load tags
    private async Task<List<Tag>> LoadTags() => await Queries.GetMoniTags(MyMoniId);
    //Load BankAccounts
    private async Task<List<BankAccount>> LoadAccounts() => await Queries.GetMoniAccounts(MyMoniId); 

    // Cast to Observable collection
    private async void InitializeAsync()
    {
        List<Transaction> transac = await LoadTransactions();
        Transactions = new ObservableCollection<Transaction>(transac);
        List<Tag> tags = await LoadTags();
        Tags = new ObservableCollection<Tag>(tags);
        List<BankAccount> accounts= await LoadAccounts();
        BankAccounts = new ObservableCollection<BankAccount>(accounts);
    }

    /// <summary>
    /// Delete the transaction from DB, if succes, deletes it from list too
    /// </summary>
    /// <param name="transacId">Id of the transaction</param>
    private async void DeleteTransaction(int transacId)
    {
        string res = await Queries.DeleteTransaction(transacId);
        if(res != null)
        {
            Transactions.Remove(Transactions.FirstOrDefault(x => x.TransactionId == transacId));
        }
    }

    /// <summary>
    /// Click to get filter to null and show all transactions
    /// </summary>
    public void ResetFilters()
    {
        SelectedTags = new();
        SelectedBankAccount = new();
    }
}
