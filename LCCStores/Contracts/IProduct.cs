using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IProduct
    {
        int Id { get; set; }
        int ProductDetailId { get; set; }
        ProductDetail ProductDetail { get; set; }
        int BrandId { get; set; }
        Brand Brand { get; set; }
        int TaxId { get; set; }
        Tax Tax { get; set; }
        int AddedById { get; set; }
        Admin AddedBy { get; set; }
        //int? ReviewId { get;set }
        //Review Review { get; set }
    }
}
