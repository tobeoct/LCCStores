using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class BillingInfo : IBillingInfo
    {
        public int Id { get; set; }
        public int CreditCardId { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}