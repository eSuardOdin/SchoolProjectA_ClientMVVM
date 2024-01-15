using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class TagsViewModel : ViewModelBase
{
    private ViewModelBase _tagsContentViewModel;
    private int MoniId { get; set; }
    public ShowTagsViewModel TagsList { get; }
    

    public ViewModelBase TagsContentViewModel
    {
        get => _tagsContentViewModel;
        set => this.RaiseAndSetIfChanged(ref _tagsContentViewModel, value);
    }
    
    public TagsViewModel(int moniId)
    {
        MoniId = moniId;
        TagsList = new(MoniId);
        TagsContentViewModel = TagsList;
    }

    /// <summary>
    /// Showing add tag form
    /// </summary>
    public async void AddTag()
    {
        AddTagViewModel addTagVM = new AddTagViewModel(MoniId);
        Observable.Merge(
            addTagVM.AddCommand,
            addTagVM.CancelCommand.Select(_=> (Tag?)null))
            .Take(1)
            .Subscribe( async newTag =>
            {
                if(newTag != null)
                {
                    newTag.MoniId = MoniId;
                    newTag = await Queries.PostTag(newTag);
                    if(newTag != null)
                    {
                        TagsList.Tags.Add(newTag);
                    }
                }
                TagsContentViewModel = TagsList;
            }
        );
        TagsContentViewModel = addTagVM;
    }

}
