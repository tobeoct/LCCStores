using LCCStores.Helper;
using LCCStores.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static LCCStores.Controllers.CartController;

namespace LCCStores.Controllers
{
    public class OrderController : ApiController
    {
        

        // POST api/Order/CreateOrder
        [HttpPost]
        [Route("api/Order/PlaceOrder")]
        public IHttpActionResult PlaceOrder(TotalOrder totalOrder)
        {
            var genericResponse = new GenericResponse();
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