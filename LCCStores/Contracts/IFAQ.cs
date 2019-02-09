using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Contracts
{
    public interface IFAQ
    {
        int Id { get; set; }
        string Question { get; set; }
        string Answer { get; set; }
        int Count { get; set; }
    }
}