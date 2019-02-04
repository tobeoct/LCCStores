using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ICategory_Product
    {
        int Id { get; set; }
        int ProductId { get; set; }
        Product Product { get; set; }
        int CategoryId { get; set; }
        Category Category { get; set; }
    }
}
