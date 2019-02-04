using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IAdminLog
    {
        int Id { get; set; }
        int AdminId { get; set; }
        Admin Admin { get; set; }
        DateTime DateLoggedIn { get; set; }
        DateTime DateLoggedOut { get; set; }
        string Action { get; set; }
    }
}
