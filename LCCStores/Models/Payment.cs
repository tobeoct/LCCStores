using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Payment : IPayment
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int BillingInfoId { get; set; }
        public DateTime Date { get; set; }
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public BillingInfo BillingInfo { get; set; }
    }
}