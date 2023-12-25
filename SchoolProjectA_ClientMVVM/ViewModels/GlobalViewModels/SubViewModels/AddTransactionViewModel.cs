using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using SchoolProjectA_ClientMVVM.Models;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class AddTransactionViewModel : ViewModelBase
{
    private string _transactionLabel;
    private string? _transactionDescription;
    private decimal _transactionAmount;
    private DateTime _transactionDate;
    private int _bankAccountId;
    private int[] _tags;
    private List<BankAccount>? _bankAccounts;
    public int MoniId {  get; set; }

    public ReactiveCommand<Unit, Transaction> AddTransactionCommand { get; set; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

    public AddTransactionViewModel(int moniId)
    {
        MoniId = moniId;
    }


}
