using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IOrderDetail
    {
        int Id { get; set; }
        int OrderId { get; set; }
        Order Order { get; set; }
        int ProductId { get; set; }
        Product Product { get; set; }
        decimal UnitPrice { get; set; }
        int Quantity { get; set; }
        int OrderNumber { get; set; }
        decimal Discount { get; set; }
    }
}
