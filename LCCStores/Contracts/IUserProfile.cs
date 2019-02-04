using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
   public interface IUserProfile
    {
        int Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        int CustomerId { get; set; }
        Customer Customer { get; set; }
        int SupplierId { get; set; }
        Supplier Supplier { get; set; }
    }

}
