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
    private string _bankAccountLabel = string.Empty;
    private decimal _bankAccountBalance = 0;

    public ReactiveCommand<Unit, BankAccount> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    /// <summary>
    /// Construcor, checking if bank account label is not null and allowing click if ok
    /// </summary>
    public AddAccountViewModel()
    {
        var isValidObservable = this.WhenAnyValue(
            x => x.BankAccountLabel,
            x => !string.IsNullOrWhiteSpace(x));
        AddCommand = ReactiveCommand.Create(
            () => CreateAccount() , isValidObservable);
        CancelCommand = ReactiveCommand.Create(
            () => { });
    }
    
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

    /// <summary>
    /// Bank account creation
    /// </summary>
    /// <returns>The bank account created in form</returns>
    private BankAccount CreateAccount()
    {
        BankAccount ba = new();
        ba.BankAccountLabel = this.BankAccountLabel;
        ba.BankAccountBalance = this.BankAccountBalance;
        System.Diagnostics.Debug.WriteLine($"{ba.BankAccountLabel.ToString()}: {ba.BankAccountBalance.ToString()}€");
        return ba;
    }
}
