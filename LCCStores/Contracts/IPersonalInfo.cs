using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IPersonalInfo
    { 
        int Id { get; set; }
        string Email { get; set; }
        string PostalCode { get; set; }
        int AddressId { get; set; }
        Address Address { get; set; }
        int PhoneNumberId { get; set; }
        PhoneNumber PhoneNumber { get; set; }
    }
}
