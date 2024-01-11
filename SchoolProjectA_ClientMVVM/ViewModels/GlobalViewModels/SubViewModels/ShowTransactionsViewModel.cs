using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using DynamicData.Binding;
using System.Reactive.Linq;


namespace SchoolProjectA_ClientMVVM.ViewModels;

public class ShowTransactionsViewModel : ViewModelBase
{
    private ObservableCollection<Transaction> _transactions;
    private ObservableCollection<Transaction> _cachedTransactions; //To backup
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
        set
        {
            this.RaiseAndSetIfChanged(ref _transactionStartDate, value);
            if(_transactionEndDate < value)
            {
                TransactionEndDate = value;
            }
        }
    }

    public DateTimeOffset TransactionEndDate
    {
        get => _transactionEndDate;
        set
        {   
            if(value < _transactionStartDate)
            {
                this.RaiseAndSetIfChanged(ref _transactionEndDate, _transactionStartDate);
                _transactionEndDate = _transactionStartDate;
            }
            else
            {
                this.RaiseAndSetIfChanged(ref _transactionEndDate, value);
            }
        }
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


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="moniId"></param>
    public ShowTransactionsViewModel(int moniId) 
    {
        MyMoniId = moniId;
        InitializeAsync();
        
        DeleteTransactionCommand = ReactiveCommand.Create<int>(DeleteTransaction);
    }

    // Init form dates with transactions dates
    private void SetDates()
    {

        TransactionStartDate = (from d in _cachedTransactions select d.TransactionDate).Min();
        System.Diagnostics.Debug.WriteLine((from d in _cachedTransactions select d.TransactionDate).Min());
        TransactionEndDate = (from d in _cachedTransactions select d.TransactionDate).Max();
        System.Diagnostics.Debug.WriteLine((from d in _cachedTransactions select d.TransactionDate).Max());
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
        _cachedTransactions = Transactions;
        List<Tag> tags = await LoadTags();
        Tags = new ObservableCollection<Tag>(tags);
        List<BankAccount> accounts= await LoadAccounts();
        BankAccounts = new ObservableCollection<BankAccount>(accounts);
        SetDates();
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
    public async void ResetFilters()
    {
        /*SelectedTags = new();
        SelectedBankAccount = new();
        TransactionStartDate = DateTime.Today;
        TransactionEndDate = DateTime.Today;
        Transactions = _cachedTransactions; // Get original transactions
        // Reset dates
        SetDates();*/
        InitializeAsync();
    }

    /// <summary>
    /// Filter the transactions depending on filters values
    /// </summary>
    public async void ApplyFilter()
    {
        Transactions = _cachedTransactions;
        ObservableCollection<Transaction> filteredTransactions = Transactions;
        // Get bank account's transactions
        if (SelectedBankAccount != null)
        {
            filteredTransactions = new ObservableCollection<Transaction>(_cachedTransactions.Where(x => x.BankAccountId == SelectedBankAccount.BankAccountId).ToList());
        }
        // Get tagged transactions
        if(SelectedTags.Count != 0)
        {
            ObservableCollection<Transaction> tempTransacs = new();
            foreach (var tag in SelectedTags)
            {
                // Get all Transactions with a tag
                ObservableCollection<Transaction> taggedTransacs = new ObservableCollection<Transaction>(await Queries.GetTaggedTransactions(tag.TagId));
                foreach (var taggedTransac in taggedTransacs)
                {
                    // Check if not already filtered by bank account
                    Transaction t = filteredTransactions.Where(x => x.TransactionId == taggedTransac.TransactionId).FirstOrDefault();
                    if(t != null)
                    {
                        tempTransacs.Add(taggedTransac);
                    }
                }
            }
            filteredTransactions = new ObservableCollection<Transaction>(tempTransacs.Distinct().ToList());
        }
        // Get by date

        List<Transaction> datedTransactions = filteredTransactions.Where(x => x.TransactionDate >= TransactionStartDate && x.TransactionDate <= TransactionEndDate).ToList();
        filteredTransactions = new ObservableCollection<Transaction>(datedTransactions);
        foreach(var transaction in datedTransactions)
        {
            System.Diagnostics.Debug.WriteLine($"{transaction.TransactionLabel}");  
        }
        Transactions = new ObservableCollection<Transaction>(filteredTransactions.Distinct().ToList());
    }
}
