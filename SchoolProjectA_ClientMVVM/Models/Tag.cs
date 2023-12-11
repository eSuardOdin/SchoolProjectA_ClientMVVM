using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.Models;
public class Tag
{
    public int TagId { get; set; }
    public string TagLabel { get; set; } = null!;
    public string? TagDescription { get; set; }
    public int MoniId { get; set; }
}