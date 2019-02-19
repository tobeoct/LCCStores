using LCCStores.Helper;
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
        Guid OrderNumber { get; set; }
        DateTime OrderDate { get; set; }
        DateTime DeliveryDate { get; set; }
        DateTime? ShippedDate { get; set; }
        ShipVia ShipVia { get; set; }
        int? CourierId { get; set; }
        Courier Courier { get; set; }
        decimal Freight { get; set; }
        int BillingInfoId { get; set; }
        BillingInformation BillingInfo { get; set; }
        OrderStatus OrderStatus { get; set; }
        decimal TotalPrice { get; set; }
    }
}
