using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IReview
    {
        int Id { get; set; }
        int ProductId { get; set; }
        Product Product { get; set; }
        int UserProfileId { get; set; }
        UserProfile UserProfile { get; set; }
        DateTime Date { get; set; }
       

    }
}
