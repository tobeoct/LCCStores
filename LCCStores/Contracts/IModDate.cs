using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IModDate
    {
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
    }
}
