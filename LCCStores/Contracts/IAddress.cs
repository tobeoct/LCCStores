using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IAddress
    {
        int Id { get; set; }
        string Street { get; set; }
        int CountryId { get; set; }
        Country Country { get; set; }
        int CityId { get; set; }
        City City { get; set; }
    }
}
