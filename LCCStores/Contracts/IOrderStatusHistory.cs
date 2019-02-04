using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IOrderStatusHistory
    {
        int Id { get; set; }
        int OrderId { get; set; }
        Order Order { get; set; }
        int OrderStatusId{ get; set; }
        OrderStatus OrderStatus { get; set; }
        int UserId { get; set; }
        DateTime Date { get; set; }
    }
}
