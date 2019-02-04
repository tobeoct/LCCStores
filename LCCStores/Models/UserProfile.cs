using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class UserProfile : IUserProfile
    {
        public int Id { get; set;}
        public string Username { get; set;}
        public string Email { get; set;}
        public string Password { get; set;}
        public int CustomerId { get; set;}
        public int SupplierId { get; set;}
        public Customer Customer { get; set;}
        public Supplier Supplier { get; set;}
    }
}