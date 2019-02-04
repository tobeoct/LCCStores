using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class ProductDetail : IProductDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitWeight { get; set; }
        public int UnitInStock { get; set; }
        public int UnitOnOrder { get; set; }
        public decimal Discount { get; set; }
        public int ReOrderLevel { get; set; }
    }
}