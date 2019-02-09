using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Review : IReview
    {
        public int Id { get; set;}
        public int ProductId { get; set;}
        public Product Product { get; set;}
        public int UserProfileId { get; set;}
        public UserProfile UserProfile { get; set;}
        public DateTime Date { get; set;}
    }
}