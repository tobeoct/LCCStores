using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IOrder
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        Customer Customer { get; set; } 
        int OrderNumber { get; set; }
        DateTime OrderDate { get; set; }
        DateTime DeliveryDate { get; set; }
        DateTime ShippedDate { get; set; }
        string ShipVia { get; set; }
        int CourierId { get; set; }
        Courier Courier { get; set; }
        decimal Freight { get; set; }
        int BillingInfoId { get; set; }
        BillingInfo BillingInfo { get; set; }
        int OrderStatusId { get; set; }
        OrderStatus OrderStatus { get; set; }
    }
}
