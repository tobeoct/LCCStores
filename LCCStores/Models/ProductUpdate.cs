using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class ProductUpdate : IUpdates
    {
        public int Id { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}