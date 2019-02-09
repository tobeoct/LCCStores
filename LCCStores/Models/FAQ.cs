using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class FAQ : IFAQ
    {
        public int Id {get; set; }
        public string Question {get; set; }
        public string Answer {get; set; }
        public int Count {get; set; }
    }
}