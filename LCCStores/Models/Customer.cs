using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonalInfoId { get; set; }
        public int BillingInfoId { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
        public BillingInfo BillingInfo { get; set; }
    }
}