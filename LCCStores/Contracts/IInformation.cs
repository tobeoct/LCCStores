using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IInformation
    {
        int Id { get; set; }
        string Privacy { get; set; }
        string ConditionForUse { get; set; }

    }
}
