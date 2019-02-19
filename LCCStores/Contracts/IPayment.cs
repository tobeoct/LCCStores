using LCCStores.Helper;
using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IPayment
    {
        int Id { get; set; }
       // PaymentType Type { get; set; }
        int OrderId { get; set; }
        Order Order { get; set; }
        int CustomerId { get; set; }
        Customer Customer { get; set; }
        int BillingInfoId { get; set; }
        BillingInformation BillingInfo { get; set; }
        DateTime Date { get; set; }
        string PaymentReference { get; set; }
        PaymentStatus Status { get; set; }
        string AuthCode { get; set; }
        string Type { get; set; }
    }
}

