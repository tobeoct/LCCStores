using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Product : IProduct,IModDate
    {
        public int Id { get; set; }
        public int ProductDetailId { get; set; }
        public int BrandId { get; set; }
        public int TaxId { get; set; }
        public ProductDetail ProductDetail {get; set; }
        public Brand Brand {get; set; }
        public Tax Tax {get; set; }
        public int AddedById {get; set; }
        public Admin AddedBy {get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}