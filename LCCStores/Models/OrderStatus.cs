using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class OrderStatus : IOrderStatus
    {
        public int Id {get; set; }
        public string Name {get; set; }
    }
}