using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;

public class ShowTagsViewModel : ViewModelBase
{
    private int MoniId {  get; set; }
    
    public ShowTagsViewModel(int moniId)
    {
        MoniId = moniId;
    }
}

