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
    private int MoniId { get; set; }

    public ReactiveCommand<Unit, Tag> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public AddTagViewModel(int moniId)
    {
        MoniId = moniId;
        var isValidObservable = this.WhenAnyValue(
            x => x.TagLabel,
            x => !string.IsNullOrWhiteSpace(x));
        AddCommand = ReactiveCommand.CreateFromTask(
            async () => await CreateTag(), isValidObservable);
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
        if(await CheckForm())
        {
            Tag tag = new();
            tag.TagLabel = TagLabel;
            tag.TagDescription = TagDescription;
            tag.MoniId = MoniId;
            return tag;
        }
        else
        {
            return null;
        }
    }

    private async Task<bool> CheckForm()
    {
        ErrorMessage = string.Empty;
        // Check for empty message
        if(TagLabel == null)
        {
            ErrorMessage = "Vous devez donner un nom à votre étiquette";
            return false;
        }
        // Check tags labels
        List<Tag> tags = await Queries.GetMoniTags(MoniId);
        Tag? tag = tags.Where(x => x.TagLabel == TagLabel).FirstOrDefault();
        if(tag != null)
        {
            ErrorMessage = "Vous avez déjà une étiquette nommé ainsi";
            return false;
        }
        return true;
    }
}