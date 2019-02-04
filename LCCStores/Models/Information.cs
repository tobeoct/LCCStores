using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Information : IInformation
    {
        public int Id {get; set; }
        public string Privacy {get; set; }
        public string ConditionForUse {get; set; }
    }
}