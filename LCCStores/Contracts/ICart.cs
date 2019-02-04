using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ICart
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        Customer Customer { get; set; }
        decimal TotalPrice { get; set; }
        int NumberOfProducts { get; set; }
    }
}
