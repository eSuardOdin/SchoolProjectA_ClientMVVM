using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class ShowTagsViewModel : ViewModelBase
{
    private int MoniId {  get; set; }
    private ObservableCollection<Tag> _tags;


    public ObservableCollection<Tag> Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }
    public ShowTagsViewModel(int moniId)
    {
        MoniId = moniId;
        InitializeAsync();
    }


    private async Task<List<Tag>> LoadTags() // Null checking ?
    {
        List<Tag> tags = await Queries.GetMoniTags(MoniId);
        return tags;
    }

    private async void InitializeAsync() => Tags = new ObservableCollection<Tag>(await LoadTags());
    
}

