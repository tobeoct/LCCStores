using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class ModDate : IModDate
    {
        public DateTime DateCreated {get; set; }
        public DateTime DateUpdated {get; set; }
    }
}