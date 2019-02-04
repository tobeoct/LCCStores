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
        int PhoneNumber1 { get; set; }
        int PhoneNumber2 { get; set; }
    }
}
