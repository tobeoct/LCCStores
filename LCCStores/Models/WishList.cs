using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class WishList : IWishList
    {
        public int Id {get; set; }
        public int CustomerId {get; set; }
        public int ProductId {get; set; }
        public int Quantity {get; set; }
        public Customer Customer {get; set; }
        public Product Product {get; set; }
    }
}