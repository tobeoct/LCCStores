using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class ReviewDetail : IReviewDetail
    {
        public int Id { get; set;}
        
        public string Comment { get; set;}
        public int ReviewId { get; set; }
        public Review Review { get; set; }
        public DateTime Date { get; set; }
    }
}