using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class AddTagViewModel : ViewModelBase
{
    private string _tagLabel = string.Empty;
    private string _tagDescription = string.Empty;
    private string _errorMessage = string.Empty;

    public ReactiveCommand<Unit, Tag> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public AddTagViewModel()
    {
        var isValidObservable = this.WhenAnyValue(
            x => x.TagLabel,
            x => !string.IsNullOrWhiteSpace(x));
        AddCommand = ReactiveCommand.Create(
            () => CreateTag(), isValidObservable);
        CancelCommand = ReactiveCommand.Create(
            () => { });
    }

    public string TagLabel
    {
        get => _tagLabel;
        set => this.RaiseAndSetIfChanged(ref _tagLabel, value);
    }

    public string TagDescription
    {
        get => _tagDescription;
        set => this.RaiseAndSetIfChanged(ref _tagDescription, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    private async Task<Tag?> CreateTag()
    {
        return new Tag();
    }
}