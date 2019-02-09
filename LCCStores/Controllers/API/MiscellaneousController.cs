using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LCCStores.Controllers
{
    public class MiscellaneousController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage GetFAQs()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "Error");
        }

      
    }
}