using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class CategoryImage : ICategoryImage
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}