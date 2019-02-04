using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ICategoryImage
    {
        int Id { get; set; }
        string Picture { get; set; }
        int CategoryId { get; set; }
        Category Category { get; set; }
    }
}
