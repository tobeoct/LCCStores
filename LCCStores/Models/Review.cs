using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Review : IReview
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ProductId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int UserProfileId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime Date { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ReviewDetailId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Product Product { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UserProfile UserProfile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ReviewDetail ReviewDetail { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}