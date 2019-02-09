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
    public class BillingController : ApiController
    {
        public class TotalBilling
        {
            public BillingInformation BillingInformation { get; set; }
            public int CustomerId { get; set; }
        }
       
        EntityLogic<BillingInformation> _entityLogic;
       
        string _errorMessage = "";

        public BillingController()
        {            
            _entityLogic= new EntityLogic<BillingInformation>();
        }

        // GET api/<controller>
        public HttpResponseMessage CreateBillingInfo(TotalBilling billingInfo)
        {
            var genericResponse = new GenericResponse();
            var customerLogic = new EntityLogic<Customer>();
            var customer = customerLogic.GetSingle(c => c.Id == billingInfo.CustomerId);
            try
            {
               
                Trace.TraceInformation("Saving Customer Billing Information");

                //VALIDATING CUSTOMER DETAILS
               ValidateBilling(billingInfo.BillingInformation, Actions.Create);
                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while saving Customer:{customer?.FirstName} Billing Information- {_errorMessage}", null);

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };
                var billingInformationToDb = billingInfo.BillingInformation;
                billingInformationToDb.Date = DateTime.Now;
               
                _entityLogic.Insert(billingInfo.BillingInformation);
                _entityLogic.Save();
                customer.BillingInfoId = billingInformationToDb.Id;
                customerLogic.Update(customer);
                new Updates().CustomersUpdate();


                Trace.TraceInformation($"Customer :{JsonConvert.SerializeObject(customer)}  Billing Information Created Successfully");
                genericResponse = new Response().GenerateResponse(true, $" Customer:{customer?.FirstName}  Billing Information added successfully", null);

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Customer:{customer?.FirstName}  Billing Information", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        public bool ValidateBilling (BillingInformation billing, Actions action)
        {
            var error = "";
            //VALIDATE CUSTOMER
            if (action == Actions.Create)
            {
                var isCart = _entityLogic.GetSingle(c => c.Id == billing.Id);
                if (isCart != null)
                {
                    error = error + "-Customer already exists-";
                }
            }
            error = error + new Validations<BillingInformation>().ValidateData(billing);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }

    }
}