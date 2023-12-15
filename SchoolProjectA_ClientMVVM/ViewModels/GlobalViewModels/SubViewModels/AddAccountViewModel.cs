using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class AddAccountViewModel : ViewModelBase
{
    public string _bankAccountLabel = string.Empty;
    public decimal _bankAccountBalance = 0;

    public ReactiveCommand<Unit, BankAccount> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }


    
    public string BankAccountLabel
    {
        get => _bankAccountLabel;
        set => this.RaiseAndSetIfChanged(ref _bankAccountLabel, value);
    }

    public decimal BankAccountBalance
    {
        get => _bankAccountBalance;
        set => this.RaiseAndSetIfChanged(ref this._bankAccountBalance, value);
    }
}
