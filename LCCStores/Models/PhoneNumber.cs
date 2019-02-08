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
        public string NumberOne {get; set; }
        public string NumberTwo {get; set; }
    }
}