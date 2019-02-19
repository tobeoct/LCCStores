using LCCStores.Contracts;
using LCCStores.Helper;
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
        public Guid OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public ShipVia ShipVia { get; set; }
        public int? CourierId { get; set; }
        public decimal Freight { get; set; }
        public int BillingInfoId { get; set; }
        public Customer Customer { get; set; }
        public Courier Courier { get; set; }
        public BillingInformation BillingInfo { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
    }
}