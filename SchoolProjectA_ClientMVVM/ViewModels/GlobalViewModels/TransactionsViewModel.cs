using Microsoft.CodeAnalysis;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class TransactionsViewModel : ViewModelBase
{
    private ViewModelBase _transactionsContentViewModel;
    public int MoniId { get; set; }
    public TransactionsViewModel(int moniId) 
    {
        MoniId = moniId;
        TransactionsContentViewModel = new ShowTransactionsViewModel(MoniId);
    }

    public ViewModelBase TransactionsContentViewModel
    {
        get => _transactionsContentViewModel;
        set => this.RaiseAndSetIfChanged(ref _transactionsContentViewModel, value);
    }

    public async void AddTransaction()
    {
        TransactionsContentViewModel = new AddTransactionViewModel(MoniId);
    }
}
