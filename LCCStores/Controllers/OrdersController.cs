using LCCStores.Helper;
using LCCStores.Logic;
using LCCStores.Models;
using Paystack.Net.SDK.Transactions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LCCStores.Controllers
{
    public class OrdersController : Controller
    {

       
        public async Task<ActionResult> Callback()
        {
            string secretKey = ConfigurationManager.AppSettings["SECRET_KEY"];
            var paystackTransactionAPI = new PaystackTransaction(secretKey);
            var tranxRef = HttpContext.Request.QueryString["reference"];
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
                        return View("Index");
                    }
                    else
                    {

                        payment.Status = PaymentStatus.UnVerififed;
                        return View("Callback");
                    }
                }
            }

            return View("Callback");
        }

        public async Task<ActionResult> WebHook()
        {
            string secretKey = ConfigurationManager.AppSettings["SECRET_KEY"];
            var paystackTransactionAPI = new PaystackTransaction(secretKey);
            var tranxRef = HttpContext.Request;
                var refe = tranxRef.QueryString["reference"];
            if (tranxRef != null)
            {
                var response = await paystackTransactionAPI.VerifyTransaction(refe);
                if (response.status)
                {
                    return View(response);
                }
            }

            return View("PaymentError");
        }
    }
}