using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Courier : ICourier
    {
        public int Id { get; set; }
        public int PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public int AddedById { get; set; }
        public Admin AddedBy { get; set; }
        public string PlateNumber { get; set; }
    }
}