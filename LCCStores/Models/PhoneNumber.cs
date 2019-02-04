using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{

    public class PhoneNumber : IPhoneNumber
    {
        public int Id {get; set; }
        public int PhoneNumber1 {get; set; }
        public int PhoneNumber2 {get; set; }
    }
}