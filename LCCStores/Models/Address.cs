using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Address : IAddress
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
    }
}