using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Tax : ITax
    {
        public int Id { get; set;}
        public string Name { get; set;}
        public decimal Value { get; set;}
    }
}