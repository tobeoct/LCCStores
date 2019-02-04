using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IOrderStatus
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
