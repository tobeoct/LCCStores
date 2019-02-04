using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class AdminLog : IAdminLog
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public DateTime DateLoggedIn { get; set; }
        public DateTime DateLoggedOut { get; set; }
        public string Action { get; set; }
        public Admin Admin { get; set; }
    }
}