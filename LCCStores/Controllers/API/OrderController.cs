using LCCStores.Helper;
using LCCStores.Logic;
using LCCStores.Models;
using Newtonsoft.Json;
using Paystack.Net.SDK.Transactions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
            try
            {
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
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting orders", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // PUT api/Order/OrderStatusChange
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/Order/OrderStatusChange")]
        public HttpResponseMessage OrderStatusChange(StatusChange status)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("UPDATING Order Status");

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

        [NonAction]
        public async Task<GenericResponse> PlaceOrder(TotalOrder totalOrder)
        {
            var genericResponse = new GenericResponse();
            var transactionResult = new TransactionResult();
            try
            {
                //PAYPAL INTEGRATION
              
                var customerModel = new PaystackCustomerModel()
                {
                    Amount = (int)(totalOrder.Order.TotalPrice * 100),
                    FirstName = totalOrder.Order.Customer.FirstName,
                    LastName = totalOrder.Order.Customer.LastName,
                    Email = totalOrder.Order.Customer.PersonalInfo.Email
                };
                transactionResult = await Pay(customerModel);

                
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while placing order", null);
                return genericResponse;
            }
            Trace.TraceInformation($"Successfully Placed Order ");
            genericResponse = new Response().GenerateResponse(true, $"Successfully Placed Order for verification", transactionResult);
            return genericResponse;
        }


        [NonAction]
        public async Task<TransactionResult> Pay(PaystackCustomerModel customerModel)
        {
            var secret_key = ConfigurationManager.AppSettings["SECRET_KEY"];
            var transactionReference = "";
            var api = new PaystackPayment(secret_key);
            var authCode = "";
            var authorisationUrl = "";
           
            //// Initializing a transaction
            var response = await api.InitializePayment(customerModel);
            if (response.status)
            {
                transactionReference = response.data.reference;
                authorisationUrl = response.data.authorization_url;
                return response;
            }
            // use response.Data
            else
            {
                return null;
            }
        }

    ////[Authorize]
    //[AcceptVerbs("POST")]
    //[HttpPost]
    //[Route("api/Order/Pay")]
    //public HttpResponseMessage Pay()
    //{
    //    var secret_key = ConfigurationManager.AppSettings["SECRET_KEY"];
    //    var transactionReference = "";
    //    var api = new PayStackApi(secret_key);
    //    var authCode = "";
    //    var authorisationUrl = "";
    //    //// Initializing a transaction
    //    var response = api.Transactions.Initialize("tobe.onyema@gmail.com", 5000000, null);
    //    if (response.Status)
    //    {
    //        transactionReference = response.Data.Reference.ToString();
    //        authorisationUrl = response.Data.AuthorizationUrl;
    //    }
    //    // use response.Data
    //    else
    //    {

    //    }
    //    // show response.Message

    //    // Verifying a transaction
    //    var verifyResponse = api.Transactions.Verify(transactionReference); // auto or supplied when initializing;
    //    if (verifyResponse.Status)
    //    {
    //        authCode = verifyResponse.Data.Authorization.AuthorizationCode;
    //        return Request.CreateResponse(HttpStatusCode.OK, authorisationUrl);

    //    }


    //    //var payment = new PaystackPayment(("Bearer " + secret_key));
    //    //var transaction = await payment.InitializeTransaction(500000, "tobe.onyema@gmail.com");
    //    //if (transaction.status)
    //    //{
    //    //    //redirect to authorization url
    //    //    authorisationUrl = transaction.data.authorization_url;
    //    //    transactionReference = transaction.data.reference;
    //    //    return Request.CreateResponse(HttpStatusCode.OK, authorisationUrl);
    //    //    //return Json(transaction, JsonRequestBehavior.AllowGet);
    //    //}

    //    //var verifyResponse = await payment.VerifyTransaction(transactionReference);

    //    //if (!String.IsNullOrEmpty(verifyResponse))
    //    //{
    //    //    //    authCode = verifyResponse.Data.Authorization.AuthorizationCode;
    //    //      return Request.CreateResponse(HttpStatusCode.OK,authorisationUrl);
    //    //}
    //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
    //}


    //[Authorize]


    [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Order/Callback")]
        public async Task<HttpResponseMessage> Callback([FromUri]string trxRef,string reference )
        {

            string secretKey = ConfigurationManager.AppSettings["SECRET_KEY"];
            var paystackTransactionAPI = new PaystackTransaction(secretKey);
            var tranxRef = reference;
            if (tranxRef != null)
            {
                var paymentLogic = new EntityLogic<Payment>();
                var orderLogic = new EntityLogic<Order>();
                var orderHistoryLogic = new EntityLogic<OrderStatusHistory>();
                var payment = paymentLogic.GetSingle(c => c.PaymentReference == tranxRef);
                if (payment != null)
                {
                    var order = orderLogic.GetSingle(c => c.Id == payment.OrderId);

                    var response = await paystackTransactionAPI.VerifyTransaction(tranxRef);
                    if (response.status)
                    {

                        payment.Status = PaymentStatus.Verified;
                        payment.Date = response.data.transaction_date;
                        payment.AuthCode = response.data.authorization.authorization_code;
                        payment.Type = response.data.authorization.card_type;
                        if (order != null)
                        {


                            order.OrderStatus = OrderStatus.Placed;
                            orderLogic.Update(order);
                            orderLogic.Save();
                            var orderStatusHistory = new OrderStatusHistory()
                            {
                                Date = DateTime.Now,
                                OrderId = order.Id,
                                OrderStatus = OrderStatus.Placed,
                                UserId = 3
                            };
                            orderHistoryLogic.Insert(orderStatusHistory);
                            orderHistoryLogic.Save();
                        }
                        paymentLogic.Save();
                        return Request.CreateResponse(HttpStatusCode.OK, "Verified");
                    }
                    else
                    {

                        payment.Status = PaymentStatus.UnVerififed;
                        return Request.CreateResponse(HttpStatusCode.OK, "Payment Unverified");
                    }
                }
            }


            return Request.CreateResponse(HttpStatusCode.OK, "Done");
        }

    }
}