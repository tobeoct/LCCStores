using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class PersonalInfo : IPersonalInfo
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string AddressId { get; set; }
    }
}