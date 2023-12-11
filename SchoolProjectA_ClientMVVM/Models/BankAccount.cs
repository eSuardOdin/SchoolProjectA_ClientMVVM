using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.Models;
public class BankAccount
{
    public int BankAccountId { get; set; }
    public string BankAccountLabel { get; set; } = null!;
    public decimal BankAccountBalance { get; set; }
    public int MoniId { get; set; }
}