using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace SchoolProjectA_ClientMVVM.ViewModels;
public class ConnexionViewModel : ViewModelBase
{
    public string MoniCreatedStatus { get; set; }
    public SolidColorBrush StatusColor { get; set; }
    public ConnexionViewModel(SolidColorBrush? color = null, string status = "") 
    {
        if(color == null)
        {
            StatusColor = new SolidColorBrush(0xFFFF0000);
        }
        StatusColor = color;
        MoniCreatedStatus = status;
    }
}

