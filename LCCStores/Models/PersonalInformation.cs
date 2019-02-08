using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class PersonalInformation : IPersonalInfo
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int PhoneNumberId { get; set; }
        public PhoneNumber PhoneNumber { get; set; }

    }
}