using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.Models;
public class Moni
{
    public int MoniId { get; set; }
    public string MoniLogin { get; set; } = null!;
    public string MoniPwd { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}


