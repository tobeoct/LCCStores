using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ICartItem
    {
        int Id { get; set; }
        int CartId { get; set; }
        Cart Cart { get; set; } 
        int ProductId { get; set; }
        Product Product { get; set; }
        int Quantity { get; set; }
    }
}
