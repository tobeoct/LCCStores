using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonalInfoId { get; set; }
        public int BillingInfoId { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public BillingInformation BillingInfo { get; set; }
    }
}