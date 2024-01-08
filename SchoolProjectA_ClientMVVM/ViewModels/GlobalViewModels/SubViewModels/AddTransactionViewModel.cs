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
    private DateTimeOffset _transactionDate = DateTime.Today;
    private ObservableCollection<Tag>? _tags;
    private ObservableCollection<Tag>? _selectedTags = new();
    private ObservableCollection<BankAccount>? _bankAccounts;
    private BankAccount? _selectedBankAccount = null;
    public int MoniId { get; set; }

    public ReactiveCommand<Unit, TransactionDTO> AddTransactionCommand { get; set; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

    public AddTransactionViewModel(int moniId)
    {
        MoniId = moniId;
        InitializeAsync(MoniId);

        // Link Command
        var isObservable = this.WhenAnyValue(
            x => x.TransactionLabel,
            x => !string.IsNullOrWhiteSpace(x));
        AddTransactionCommand = ReactiveCommand.Create(
            () => CreateTransaction(), isObservable); // Penser à cast en Transaction pour la liste du ShowTransacVM
        CancelCommand = ReactiveCommand.Create(
            () => { });
    }

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

    public DateTimeOffset TransactionDate
    {
        get => _transactionDate;
        set => this.RaiseAndSetIfChanged(ref _transactionDate, value);
    }

    public ObservableCollection<Tag>? Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
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

    /// <summary>
    /// Load accounts
    /// </summary>
    /// <param name="moniId"></param>
    /// <returns></returns>
    private async Task<List<BankAccount>> LoadBankAccounts(int moniId)
    {
        List<BankAccount> bankAccounts = await Queries.GetMoniAccounts(moniId);
        return bankAccounts;
    }
    private async Task<List<Tag>> LoadTags(int moniId)
    {
        List<Tag> bankAccounts = await Queries.GetMoniTags(moniId);
        return bankAccounts;
    }

    /// <summary>
    /// Casting list to ObservableCollection
    /// </summary>
    /// <param name="moniId"></param>
    private async void InitializeAsync(int moniId)
    {
        // Casting accounts
        List<BankAccount> accounts = await LoadBankAccounts(moniId);
        BankAccounts = new ObservableCollection<BankAccount>(accounts);

        // Casting tags
        List<Tag> tags = await LoadTags(moniId);
        Tags = new ObservableCollection<Tag>(tags);
    }



    private TransactionDTO CreateTransaction()
    {
        System.Diagnostics.Debug.WriteLine($"Sur le compte'{SelectedBankAccount.BankAccountLabel}':\n\tTransaction: {TransactionLabel}->{TransactionAmount}€\n{TransactionDescription}\n\tBalises:");
        foreach(var t in SelectedTags)
        {
            System.Diagnostics.Debug.WriteLine(t.TagLabel);
        }
        System.Diagnostics.Debug.WriteLine(TransactionDate);

        // Getting the selected tags id
        int[] tags = new int[SelectedTags.Count];
        int i = 0;
        foreach (var tag in SelectedTags)
        {
            tags[i] = tag.TagId;
            i++;
        }
        TransactionDTO transac = new(tags, SelectedBankAccount.BankAccountId,TransactionAmount, TransactionDate.DateTime, TransactionLabel, TransactionDescription);
        return transac;
    }

}
