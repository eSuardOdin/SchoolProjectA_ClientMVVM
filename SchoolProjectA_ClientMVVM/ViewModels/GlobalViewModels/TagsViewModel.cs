using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
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

}
