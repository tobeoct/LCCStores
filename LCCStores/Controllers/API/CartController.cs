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

namespace LCCStores.Controllers
{
    public class CartController : ApiController
    {
      
        EntityLogic<Cart> _entityLogic;
        EntityLogic<CartItem> _entityLogicItem;

        string _errorMessage = "";

        public CartController()
        {
            _entityLogic = new EntityLogic<Cart>();
            _entityLogicItem = new EntityLogic<CartItem>();

        }

        public class Carts
        {
            public Cart Cart { get; set; }
            public List<CartItem> CartItems { get; set; }

        }
        public class RCart
        {
            public Cart Cart { get; set; }
            public CartItem CartItem { get; set; }

        }
        public class TotalOrder
        {
            public Order Order { get; set; }
            public List<OrderDetail> OrderDetails { get; set; }
            public OrderStatusHistory OrderStatusHistory { get; set; }
        }

        // GET api/Cart/GetCart/2
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Cart/GetCart")]
        public HttpResponseMessage GetCart(int customerId)
        {
            var genericResponse = new GenericResponse();
            var cartsKey = $"TotalCarts{customerId}";
            var cartItems = new List<CartItem>();
            var fullCart = new Carts();
            try
            {

                Trace.TraceInformation($"Getting Cart with ID :{customerId}");
                var updateTime = new EntityLogic<CartUpdate>().GetSingle(c => c.Id == 1)?.LastUpdateTime;
                if (updateTime != null)
                {
                    if (updateTime < DateTime.Now)
                    {
                        fullCart = (Carts)new Cacher().GetCache(cartsKey);
                        if (fullCart != null)
                        {
                            genericResponse = new Response().GenerateResponse(true, "Successfully gotten all Cart Items", fullCart);

                            Trace.TraceInformation("Sending all Cart Items");

                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                        }
                    }

                }
                var cart = _entityLogic.GetSingle(c => c.Customer.Id == customerId, c => c.Customer);

                if (cart != null)
                {
                    cartItems = _entityLogicItem.GetWhere(c => c.CartId == cart.Id);
                    if (cartItems != null)
                    {
                        fullCart = new Carts();
                        fullCart.Cart = cart;
                        fullCart.CartItems = cartItems;
                    }
                }
                else
                {

                    genericResponse = new Response().GenerateResponse(false, "An error occured while getting cart - No such cart exists", null);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                }

                Trace.TraceInformation($"Gotten Cart with CustomerID :{JsonConvert.SerializeObject(fullCart)}");

                new Cacher().InsertIntoCache(cartsKey, fullCart);

                genericResponse = new Response().GenerateResponse(true, $"Successfully got Cart for :{cart.Customer.FirstName.ToUpper()}", fullCart);

                Trace.TraceInformation($"Sending Cart with CustomerID :{customerId}");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting cart", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/Cart/CreateCart
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Cart/CreateCart")]
        public HttpResponseMessage CreateCart(Carts fullCart)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Saving Cart");

                //VALIDATING CART DETAILS
                ValidateCart(fullCart.Cart, Actions.Create);
                foreach (var item in fullCart.CartItems)
                {
                    ValidateCartItem(item, Actions.Create);
                }
                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Cart for Customer:{fullCart.Cart?.Customer.FirstName}- {_errorMessage}", null);

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //SAVE Cart TO DB
                Trace.TraceInformation("Saving Cart to DB");


                fullCart.Cart.Customer = null;
                _entityLogic.Insert(fullCart.Cart);
                _entityLogic.Save();

                foreach (var item in fullCart.CartItems)
                {
                    item.CartId = fullCart.Cart.Id;
                    _entityLogicItem.Insert(item);
                    _entityLogic.Save();

                }
                new Updates().CartsUpdate();


                Trace.TraceInformation($"Cart :{JsonConvert.SerializeObject(fullCart)} Created Successfully");
                genericResponse = new Response().GenerateResponse(true, $" Cart added successfully", null);

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Cart", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/Cart/CreateCart
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Cart/AddToCart")]
        public HttpResponseMessage AddToCart(RCart fullCart)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Updating Cart");

                //VALIDATING CART DETAILS

                ValidateCartItem(fullCart.CartItem, Actions.Edit);

                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Cart for Customer:{fullCart.Cart?.Customer.FirstName}- {_errorMessage}", null);

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //SAVE Cart TO DB
                Trace.TraceInformation("Updating Cart in DB");


                fullCart.Cart.Customer = null;
                var price = fullCart.CartItem.Product.ProductDetail.UnitPrice * fullCart.CartItem.Quantity;

                //Increment Number of Products In Cart
                fullCart.Cart.NumberOfProducts = fullCart.Cart.NumberOfProducts + fullCart.CartItem.Quantity;
                fullCart.Cart.TotalPrice = fullCart.Cart.TotalPrice + price;
                var cartItemProduct = _entityLogicItem.GetWhere(c => c.ProductId == fullCart.CartItem.ProductId).FirstOrDefault();
                if (cartItemProduct == null)
                {
                    fullCart.CartItem.CartId = fullCart.Cart.Id;
                    _entityLogicItem.Insert(fullCart.CartItem);
                    _entityLogic.Save();
                }
                else
                {

                    cartItemProduct.Quantity = cartItemProduct.Quantity + fullCart.CartItem.Quantity;
                    _entityLogicItem.Update(cartItemProduct);
                    _entityLogic.Save();
                }
                _entityLogic.Update(fullCart.Cart);
                _entityLogic.Save();

                new Updates().CartsUpdate();


                Trace.TraceInformation($"Cart :{JsonConvert.SerializeObject(fullCart)} Updated Successfully");
                genericResponse = new Response().GenerateResponse(true, $" Cart updated successfully", null);

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Cart", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/Cart/CreateCart
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Cart/CheckOutCart")]
        public HttpResponseMessage CheckOutCart(Carts fullCart)
        {
            //Get Cart Details
            //Generate Order and Order Details
            //Place Order
            //Generate Payment Info
            //Send Mail to Customer and Admin
            var genericResponse = new GenericResponse();
            Trace.TraceInformation($"Checking Out Cart :{JsonConvert.SerializeObject(fullCart)}");
            try
            {
                var customer = new EntityLogic<Customer>().GetSingle(c => c.Id == fullCart.Cart.CustomerId, c => c.BillingInfo);
                if (customer != null)
                {
                    if (customer.BillingInfo != null)
                    {
                        var order = GenerateOrderDetails(fullCart, customer.BillingInfoId);
                        var isSuccessful = PlaceOrder(order);
                        if (isSuccessful)
                        {
                            var paymentInfo = GeneratePaymentInfo(order.Order, customer);
                            var savePaymentInfo = new EntityLogic<Payment>();
                            savePaymentInfo.Insert(paymentInfo);
                            savePaymentInfo.Save();
                            var invoice = GenerateInvoice();
                            if (invoice != null)
                            {
                                SendMail(customer.PersonalInfo.Email, invoice);
                            }
                        }


                    }
                }



                genericResponse = new Response().GenerateResponse(true, $"Successfully checked out Cart for :{fullCart.Cart.Customer.FirstName.ToUpper()}", fullCart);



                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while checking out cart", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }


        // PUT api/Cart/EditCart
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/Cart/EditCart")]
        public HttpResponseMessage EditCart(RCart fullCart)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("UPDATING Cart");

                //VALIDATING CART DETAILS                
                ValidateCartItem(fullCart.CartItem, Actions.Edit);
                var cart = _entityLogic.GetSingle(c => c.Id == fullCart.Cart.Id);
                var cartItem = _entityLogicItem.GetSingle(c => c.Id == fullCart.CartItem.Id,c=>c.Product,c=>c.Product.ProductDetail);
                if(cartItem!=null)
                {
                    var cartQuantity = cartItem.Quantity;
                    cartItem.Quantity = fullCart.CartItem.Quantity;
                    _entityLogicItem.Update(cartItem);
                    _entityLogicItem.Save();

                    if (cartQuantity < fullCart.CartItem.Quantity)
                    {
                        cart.NumberOfProducts = cart.NumberOfProducts + (fullCart.CartItem.Quantity - cartQuantity);
                        cart.TotalPrice = cart.TotalPrice + ((fullCart.CartItem.Quantity - cartQuantity) * cartItem.Product.ProductDetail.UnitPrice);
                    }
                    else
                    {
                        cart.NumberOfProducts = cart.NumberOfProducts - (cartQuantity - fullCart.CartItem.Quantity);
                        cart.TotalPrice = cart.TotalPrice - ((cartQuantity - fullCart.CartItem.Quantity) * cartItem.Product.ProductDetail.UnitPrice);
                    }
                    _entityLogic.Update(cart);
                    _entityLogic.Save();
                    new Updates().CartsUpdate();

                    Trace.TraceInformation($"Cart:{JsonConvert.SerializeObject(fullCart)}");

                    genericResponse = new Response().GenerateResponse(true, $"Successfully updated Cart", null);

                    Trace.TraceInformation($"Cart Updated");

                    return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                }

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating cart Item - No such cart item exist", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating cart Item", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // DELETE api/Cart/RemoveFromCart
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/Cart/RemoveFromCart")]
        public HttpResponseMessage RemoveFromCart(Index indexes)
        {
            var genericResponse = new GenericResponse();
            
            try
            {
               
                foreach (var id in indexes.Ids)
                {
                    var cartItem = _entityLogicItem.GetSingle(c => c.Id == id.Id,c=>c.Product,c=>c.Product.ProductDetail);
                   
                    if (cartItem != null)
                    {
                        var quantity = cartItem.Quantity;
                        var price = cartItem.Product.ProductDetail.UnitPrice;
                        var cart = _entityLogic.GetSingle(c => c.Id == cartItem.CartId);
                        _entityLogicItem.Delete(cartItem);
                        _entityLogicItem.Save();
                        cart.NumberOfProducts = cart.NumberOfProducts - quantity;
                        cart.TotalPrice = cart.TotalPrice - (quantity*price);
                        _entityLogic.Update(cart);
                        _entityLogic.Save();
                    }
                    else
                    {
                        genericResponse = new Response().GenerateResponse(false, $"Cart Item with Id: {id.Id} doesnt exist in the db ", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }
                }

                new Updates().CartsUpdate();
                genericResponse = new Response().GenerateResponse(true, $"Successfully removed items from cart ", null);

                Trace.TraceInformation($"Cart Items deleted");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));



            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting item from cart", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // DELETE api/Cart/DeleteCart
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/Cart/DeleteCart")]
        public HttpResponseMessage DeleteCart(int customerId)
        {
            var genericResponse = new GenericResponse();

            try
            {
                var cart = _entityLogic.GetSingle(c => c.CustomerId == customerId);

                if (cart != null)
                {
                    _entityLogic.Delete(cart);
                    _entityLogic.Save();
                    var cartId = cart.Id;
                    var cartItems = _entityLogicItem.GetWhere(c => c.CartId == cartId);

                    foreach (var item in cartItems)
                    {

                        _entityLogicItem.Delete(item);
                        _entityLogicItem.Save();

                    }

                    new Updates().CartsUpdate();
                    genericResponse = new Response().GenerateResponse(true, $"Successfully deleted cart ", null);

                    Trace.TraceInformation($"Cart deleted");

                    return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                }


                genericResponse = new Response().GenerateResponse(false, $"Customer has no cart ", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));

            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting cart", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        public TotalOrder GenerateOrderDetails(Carts fullCart, int? billingInfoId)
        {
            var totalOrder = new TotalOrder();
            try
            {
                // CREATE ORDER
                var order = new Order()
                {
                    CustomerId = fullCart.Cart.CustomerId,
                    OrderNumber = Guid.NewGuid(),
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(3),
                    ShippedDate = null,
                    ShipVia = ShipVia.Bus,
                    CourierId = null,
                    Freight = fullCart.Cart.NumberOfProducts,
                    BillingInfoId = (int)billingInfoId,
                    OrderStatus = Helper.OrderStatus.Placed

                };
                //SAVE TO DB
                var orderToDb = new EntityLogic<Order>();
                orderToDb.Insert(order);
                orderToDb.Save();

                //CREATE ORDER DETAILS
                foreach (var item in fullCart.CartItems)
                {
                    var orderDetails = new OrderDetail()
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        UnitPrice = item.Product.ProductDetail.UnitPrice,
                        Quantity = item.Quantity,
                        OrderNumber = order.OrderNumber,
                        Discount = 0,
                        Date=DateTime.Now
                    };
                    var orderDetailsToDb = new EntityLogic<OrderDetail>();
                    orderDetailsToDb.Insert(orderDetails);
                    orderDetailsToDb.Save();
                    totalOrder.OrderDetails = new List<OrderDetail>();
                    totalOrder.OrderDetails.Add(orderDetails);
                }

                //CREATE ORDER STATUS HISTORY
                var orderStatusHistory = new OrderStatusHistory()
                {
                    OrderId = order.Id,
                    OrderStatus = OrderStatus.Placed,
                    UserId = 3,
                    Date = DateTime.Now
                };

                var orderStatusHistoryToDb = new EntityLogic<OrderStatusHistory>();
                orderStatusHistoryToDb.Insert(orderStatusHistory);
                orderStatusHistoryToDb.Save();

                new Updates().OrdersUpdate();

                totalOrder.Order = order;
                totalOrder.OrderStatusHistory = orderStatusHistory;



            }
            catch (Exception e)
            {
                return null;
            }

            return totalOrder;
        }

        public bool PlaceOrder(TotalOrder totalOrder)
        {
            string url = "http://localhost:58426/api/Order/PlaceOrder/";
            var result = new ApiPostAndGet().UrlPost(url, totalOrder, null);
            var response = JsonConvert.DeserializeObject<GenericResponse>(result);

            return response.IsSuccessful;
        }

        public Payment GeneratePaymentInfo(Order order, Customer customer)
        {
            var paymentInfo = new Payment();
            if (order != null && customer != null)
            {
                paymentInfo = new Payment()
                {
                    Type = PaymentType.MasterCard,
                    OrderId = order.Id,
                    CustomerId = customer.Id,
                    BillingInfoId = (int)customer.BillingInfoId,
                    Date = DateTime.Now,
                    PaymentReference = Guid.NewGuid()
                };
            }

            return paymentInfo;
        }

        public string GenerateInvoice()
        {
            return null;
        }

        public bool SendMail(string email, string invoice)
        {
            return true;
        }

        public bool ValidateCart(Cart cart, Actions action)
        {
            var error = "";
            //VALIDATE CUSTOMER
            if (action == Actions.Create)
            {
                var isCart = _entityLogic.GetSingle(c => c.CustomerId == cart.CustomerId);
                if (isCart != null)
                {
                    error = error + "-Customer already has a cart-";
                }
            }
            error = error + new Validations<Cart>().ValidateData(cart);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }

        public bool ValidateCartItem(CartItem cartItem, Actions action)
        {
            var error = "";
            //VALIDATE CUSTOMER
            //if (action == Actions.Create)
            //{
            //    var isCart = _entityLogicItem.GetSingle(c => c.ProductId == cartItem.ProductId);
            //    if (isCart != null)
            //    {

            //        isCart.Quantity = isCart.Quantity + 1;
            //    }
            //}
            error = error + new Validations<CartItem>().ValidateData(cartItem);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }
    }
}