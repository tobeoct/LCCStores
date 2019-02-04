using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IBillingInfo
    {
        int Id { get; set; }
        int CreditCardId { get; set; }
        CreditCard CreditCard { get; set; }
        string Address { get; set; }
        DateTime Date { get; set; }
    }
}
