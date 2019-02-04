using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Category_Product : ICategory_Product
    {
        public int Id {get; set; }
        public int ProductId {get; set; }
        
        public int CategoryId {get; set; }
        public Product Product {get; set; }
        public Category Category {get; set; }
    }
}