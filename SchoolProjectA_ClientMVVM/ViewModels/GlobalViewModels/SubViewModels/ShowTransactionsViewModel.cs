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
    private int MyMoniId { get; set; }
    public ReactiveCommand<int, Unit> DeleteTransactionCommand { get; }
    public ShowTransactionsViewModel(int moniId) 
    {
        MyMoniId = moniId;
        InitializeAsync();
        DeleteTransactionCommand = ReactiveCommand.Create<int>(DeleteTransaction);
    }
    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set => this.RaiseAndSetIfChanged(ref this._transactions, value);
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

    // Cast to Observable collection
    private async void InitializeAsync()
    {
        List<Transaction> transac = await LoadTransactions();
        Transactions = new ObservableCollection<Transaction>(transac);
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
}
