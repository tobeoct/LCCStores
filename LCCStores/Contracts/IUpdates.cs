using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IUpdates
    {
        DateTime LastUpdateTime { get; set; }
    }
}
