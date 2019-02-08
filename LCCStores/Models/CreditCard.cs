using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class CreditCard : ICreditCard
    {
        public int Id { get; set; }
        public string PAN { get; set; }
        public string Pin { get; set; }
        public string CVV { get; set; }
        public string ExpiryDate { get; set; }
        public int BillingInfoId { get; set; }
        public BillingInformation BillingInfo { get; set; }
    }
}