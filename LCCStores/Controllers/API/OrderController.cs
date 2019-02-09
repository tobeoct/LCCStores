using LCCStores.Helper;
using LCCStores.Logic;
using LCCStores.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static LCCStores.Controllers.CartController;

namespace LCCStores.Controllers
{
    public class OrderController : ApiController
    {
       
        //PAYPAL INTEGRATION

        EntityLogic<Order> _entityLogic;
        EntityLogic<OrderDetail> _entityLogicDetail;

        string _errorMessage = "";

        public OrderController()
        {
            _entityLogic = new EntityLogic<Order>();
            _entityLogicDetail = new EntityLogic<OrderDetail>();

        }
        public class ROrder
        {
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public bool IsDate { get; set; }
        }
        public class TOrders
        {
            public List<Order> Orders { get; set; }
            public List<OrderDetail> OrderDetails { get; set; }
        }
        public class StatusChange
        {
            public OrderStatus OrderStatus { get; set; }
            public int OrderId { get; set; }
        }
        // GET api/Order/GetOrders
        [HttpGet]
        [Route("api/Order/GetOrders")]
        public HttpResponseMessage GetOrders(ROrder rOrder)
        {
            var genericResponse = new GenericResponse();

            var orders = new TOrders();
            var ordersKey = "";
            if (!rOrder.IsDate)
            {
                ordersKey = $"TotalOrders{DateTime.Now.AddMonths(-2).Date.DayOfYear}:{ DateTime.Now.Date.DayOfYear}";
                var updateTime = new EntityLogic<OrdersUpdate>().GetSingle(c => c.Id == 1)?.LastUpdateTime;
                if (updateTime != null)
                {
                    if (updateTime < DateTime.Now)
                    {
                        orders = (TOrders)new Cacher().GetCache(ordersKey);
                        if (orders != null)
                        {
                            genericResponse = new Response().GenerateResponse(true, $"Successfully gotten Orders from {DateTime.Now.AddMonths(-2).Date} to {DateTime.Now.Date}", orders);

                            Trace.TraceInformation("Sending orders");

                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                        }
                    }

                }
                orders = new TOrders();
                var order = _entityLogic.GetWhere(c => c.OrderDate >= DateTime.Now.AddMonths(-2) && c.OrderDate <= DateTime.Now);
                var orderDetails = _entityLogicDetail.GetWhere(c => c.Date >= DateTime.Now.AddMonths(-2) && c.Date <= DateTime.Now);
                orders.OrderDetails = orderDetails;
                orders.Orders = order;
            }
            else
            {
                ordersKey = $"TotalOrders{rOrder.FromDate}:{rOrder.ToDate}";
                var updateTime = new EntityLogic<OrdersUpdate>().GetSingle(c => c.Id == 1)?.LastUpdateTime;
                if (updateTime != null)
                {
                    if (updateTime < DateTime.Now)
                    {
                        orders = (TOrders)new Cacher().GetCache(ordersKey);
                        if (orders != null)
                        {
                            genericResponse = new Response().GenerateResponse(true, $"Successfully gotten Orders from {DateTime.Now.AddMonths(-2).Date} to {DateTime.Now.Date}", orders);

                            Trace.TraceInformation("Sending orders");

                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                        }
                    }

                }

                var order = _entityLogic.GetWhere(c => c.OrderDate >= rOrder.FromDate && c.OrderDate <= rOrder.ToDate, c => c.Courier, c => c.BillingInfo, c => c.Customer);
                var orderDetails = _entityLogicDetail.GetWhere(c => c.Date >= rOrder.FromDate && c.Date <= rOrder.ToDate, c => c.Product, c => c.Product.ProductDetail);
                orders.OrderDetails = orderDetails;
                orders.Orders = order;
            }
            new Cacher().InsertIntoCache(ordersKey, orders);
            genericResponse = new Response().GenerateResponse(true, $"Successfully gotten Orders from {DateTime.Now.AddMonths(-2).Date} to {DateTime.Now.Date}", orders);
            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
        }


        // PUT api/Cart/EditCart
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/Order/OrderStatusChange")]
        public HttpResponseMessage OrderStatusChange(StatusChange status)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("UPDATING Order Status");

                //VALIDATING CART DETAILS                
                // ValidateCartItem(fullCart.CartItem, Actions.Edit);
                var order = _entityLogic.GetSingle(c => c.Id == status.OrderId);
                if (order != null)

                {
                    order.OrderStatus = status.OrderStatus;
                    _entityLogic.Update(order);
                    _entityLogic.Save();
                    var historyLogic = new EntityLogic<OrderStatusHistory>();
                    var history = new OrderStatusHistory()
                    {
                        OrderId = order.Id,
                        OrderStatus = order.OrderStatus,
                        Date = DateTime.Now,
                        UserId = 3
                    };

                    historyLogic.Insert(history);
                    historyLogic.Save();
                    new Updates().OrdersUpdate();

                    Trace.TraceInformation($"Order:{JsonConvert.SerializeObject(status)}");

                    genericResponse = new Response().GenerateResponse(true, $"Successfully updated Order Status", null);

                    Trace.TraceInformation($"Order Updated");

                    return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                }

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating order status - No such order exist", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating order status", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/Order/PlaceOrder
        [HttpPost]
        [Route("api/Order/PlaceOrder")]
        public IHttpActionResult PlaceOrder(TotalOrder totalOrder)
        {
            var genericResponse = new GenericResponse();
            try
            {
                //PAYPAL INTEGRATION
            }catch(Exception e)
            {

            }
            genericResponse = new Response().GenerateResponse(true, $"Successfully Placed Order", totalOrder);
            return Json(genericResponse);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}