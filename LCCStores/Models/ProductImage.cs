using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class ProductImage : IProductImage
    {
        public int Id {get; set; }
        public string Name {get; set; }
        public int Order {get; set; }
        public int ProductId {get; set; }
        public Product Product {get; set; }
        public string Picture { get; set; }
    }
}