using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Supplier : ISupplier
    {
        public int Id { get; set;}
        public int PersonalInfoId { get; set;}
        public string CompanyName { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
     
        public PersonalInformation PersonalInfo { get; set;}
        public int AddedById { get; set;}
        public Admin AddedBy { get; set;}
    }
}