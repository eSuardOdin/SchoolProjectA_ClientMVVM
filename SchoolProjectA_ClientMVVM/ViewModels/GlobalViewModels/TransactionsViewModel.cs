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
    public TransactionsViewModel(int moniId) 
    {
        TransactionsContentViewModel = new ShowTransactionsViewModel(moniId);
    }

    public ViewModelBase TransactionsContentViewModel
    {
        get => _transactionsContentViewModel;
        set => this.RaiseAndSetIfChanged(ref _transactionsContentViewModel, value);
    }
}
