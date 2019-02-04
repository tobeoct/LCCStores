using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ICourier
    {
        int Id { get; set; }
        int PhoneNumber { get; set; }
        string CompanyName { get; set; }
        int AddedById { get; set; }
        Admin AddedBy { get; set; }
    }
}
