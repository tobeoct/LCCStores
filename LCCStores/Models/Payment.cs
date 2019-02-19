using LCCStores.Contracts;
using LCCStores.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Payment : IPayment
    {
        public int Id { get; set; }
      //  public PaymentType Type { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int BillingInfoId { get; set; }
        public DateTime Date { get; set; }
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public BillingInformation BillingInfo { get; set; }
        public string PaymentReference { get; set; }
        public PaymentStatus Status { get; set; }
        public string AuthCode { get; set; }
        public string Type { get; set; }
    }
}