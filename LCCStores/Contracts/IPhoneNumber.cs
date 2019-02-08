using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IPhoneNumber
    {
        int Id { get; set; }
        string NumberOne { get; set; }
        string NumberTwo { get; set; }
    }
}
