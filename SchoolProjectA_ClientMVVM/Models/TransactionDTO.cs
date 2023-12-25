using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.Models;
public class TransactionDTO
{
    public int[] Tags { get; set; }
    public int BankAccountId { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionLabel { get; set; } = null!;
    public string? TransactionDescription { get; set; }

}
