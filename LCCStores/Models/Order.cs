using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class Order : IOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string ShipVia { get; set; }
        public int CourierId { get; set; }
        public decimal Freight { get; set; }
        public int BillingInfoId { get; set; }
        public int OrderStatusId { get; set; }
        public Customer Customer { get; set; }
        public Courier Courier { get; set; }
        public BillingInfo BillingInfo { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}