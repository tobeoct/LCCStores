using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Helper
{
    public class Enums
    {

    }
    public enum StatusCode
    {
        Success = 00,
        NoExisting=01,
        Failure = 99
    }
    public enum Actions
    {
        View=0,
        Create = 1,
        Edit = 2,
        Delete=3
       
    }
    public enum OrderStatus
    {
        Placed=0,
       InDelivery=1,
       Delivered=2,
       Cancelled=3,
        YetToConfirm = 4

    }
    public enum ShipVia
    {
        Bike=0,
        Car=1,
        Bus=2

    }
    public enum PaymentType
    {
        MasterCard = 0,
        Visa = 1,
        Verve = 2

    }
    public enum PaymentStatus
    {
        Placed = 0,
        Verified = 1,
        UnVerififed = 2

    }
}