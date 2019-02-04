using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ICity
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
    }
}
