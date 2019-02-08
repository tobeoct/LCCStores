using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class BillingInformation : IBillingInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AddressId { get; set; }
        public int PhoneNumberId { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public Address Address { get; set; }
    }

}