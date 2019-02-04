using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IProductDetail
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int QuantityPerUnit { get; set; }
        decimal UnitPrice { get; set; }
        decimal UnitWeight { get; set; }
        int UnitInStock { get; set; }
        int UnitOnOrder { get; set; }
        decimal Discount { get; set; }
        int ReOrderLevel { get; set; }
    }
}
