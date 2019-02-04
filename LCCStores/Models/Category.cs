using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AddedById {get; set; }
        public Admin AddedBy {get; set; }
    }
}