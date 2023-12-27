using Microsoft.CodeAnalysis;
using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class TransactionsViewModel : ViewModelBase
{
    private ViewModelBase _transactionsContentViewModel;
    public int MoniId { get; set; }
    public ShowTransactionsViewModel ShowTransactions { get; }
    public TransactionsViewModel(int moniId) 
    {
        MoniId = moniId;
        ShowTransactions = new(MoniId);
        TransactionsContentViewModel = ShowTransactions;
    }

    public ViewModelBase TransactionsContentViewModel
    {
        get => _transactionsContentViewModel;
        set => this.RaiseAndSetIfChanged(ref _transactionsContentViewModel, value);
    }

    public async void AddTransaction()
    {
        //TransactionsContentViewModel = new AddTransactionViewModel(MoniId);
        AddTransactionViewModel addTransacVM = new(MoniId);
        Observable.Merge(
            addTransacVM.AddTransactionCommand,
            addTransacVM.CancelCommand.Select(_=>(TransactionDTO?)null))
            .Take(1)
            .Subscribe(async newTransac =>
            {
                if(newTransac != null)
                {
                    // Post dans la bdd
                    //newTransac = await Queries.PostTransaction()
                    // Cast et ajout dans la list
                    Transaction transac = await Queries.PostTransaction(newTransac);
                    if(transac != null)
                    {
                        ShowTransactions.Transactions.Add(transac);
                    }
                }
                TransactionsContentViewModel = ShowTransactions;
            });
        TransactionsContentViewModel = addTransacVM;
    }
}
