using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
   public interface ICreditCard
    {
        int Id { get; set; }
        string PAN { get; set; }
        string Pin { get; set; }
        string CVV { get; set; }
        string ExpiryDate { get; set; }
        int BillingInfoId { get; set; }
        BillingInformation BillingInfo { get; set; }
    }
}
