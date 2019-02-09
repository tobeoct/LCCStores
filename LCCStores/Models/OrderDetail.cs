using LCCStores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{

    public class OrderDetail : IOrderDetail
    {
        public int Id {get; set; }
        public int OrderId {get; set; }
        public int ProductId {get; set; }
        public decimal UnitPrice {get; set; }
        public int Quantity {get; set; }
        public Guid OrderNumber {get; set; }
        public decimal Discount {get; set; }
        public Order Order {get; set; }
        public Product Product {get; set; }
        public DateTime Date { get; set; }
    }
}