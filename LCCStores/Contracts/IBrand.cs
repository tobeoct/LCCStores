using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IBrand
    {
        int Id { get; set; }
        string Name { get; set; }
        int AddedById { get; set; }
        Admin AddedBy { get; set; }
    }
}
