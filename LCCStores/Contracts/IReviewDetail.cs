using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IReviewDetail
    {
        int Id { get; set; }
        int ReviewId { get; set; }
        Review Review { get; set; }
        string Comment { get; set; }
    }
}
