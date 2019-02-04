using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IProductImage
    {
        int Id { get; set; }
        string Name { get; set; }
        int Order { get; set; }
        int ProductId { get; set; }
        string Picture { get; set; }
        Product Product { get; set; }
    }
}
