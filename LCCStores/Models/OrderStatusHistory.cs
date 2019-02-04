using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class OrderStatusHistory : IOrderStatusHistory
    {
        public int Id {get; set; }
        public int OrderId {get; set; }
        public int UserId {get; set; }
        public DateTime Date {get; set; }
        public Order Order {get; set; }
        public int OrderStatusId {get; set; }
        public OrderStatus OrderStatus {get; set; }
    }
}