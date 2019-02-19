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
    public class CourierController : ApiController
    {

        EntityLogic<Courier> _entityLogic;

        string _errorMessage = "";

        public CourierController()
        {
            _entityLogic = new EntityLogic<Courier>();


        }

        // GET api/Courier/GetCouriers
        [HttpGet]
        [Route("api/Courier/GetCouriers")]
        public HttpResponseMessage GetCouriers()
        {
            var genericResponse = new GenericResponse();
            try
            {
                var couriers = new List<Courier>();
                var couriersKey = $"TotalCouriers";
                var updateTime = new EntityLogic<CouriersUpdate>().GetSingle(c => c.Id == 1)?.LastUpdateTime;
                if (updateTime != null)
                {
                    if (updateTime < DateTime.Now)
                    {
                        couriers = (List<Courier>)new Cacher().GetCache(couriersKey);
                        if (couriers != null)
                        {
                            genericResponse = new Response().GenerateResponse(true, $"Successfully gotten couriers", couriers);

                            Trace.TraceInformation("Sending couriers");

                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                        }
                    }

                }
                couriers = new List<Courier>();
                couriers = _entityLogic.GetAll();


                new Cacher().InsertIntoCache(couriersKey, couriers);
                genericResponse = new Response().GenerateResponse(true, $"Successfully gotten all couriers", couriers);
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting couriers", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/Courier/CreateCourier
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Courier/CreateCourier")]
        public HttpResponseMessage CreateCourier(Courier courier)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Saving Courier");

                //VALIDATING COURIER
                ValidateCourier(courier, Actions.Create);

                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while creating Courier {courier.CompanyName} - {_errorMessage}", null);

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //SAVE COURIER TO DB

                Trace.TraceInformation("Saving Cart to DB");

                courier.AddedBy = null;
                _entityLogic.Insert(courier);
                _entityLogic.Save();

                new Updates().CouriersUpdate();


                Trace.TraceInformation($"Courier :{JsonConvert.SerializeObject(courier)} Created Successfully");
                genericResponse = new Response().GenerateResponse(true, $" Courier created successfully", null);

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while creating Courier", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/Courier/EditCourier")]
        public HttpResponseMessage EditCourier(Courier courier)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("UPDATING Order Status");

                //VALIDATING COURIER               
                ValidateCourier(courier, Actions.Edit);
                var courierInDb = _entityLogic.GetSingle(c => c.Id == courier.Id);
                if (courierInDb != null)

                {
                    courierInDb.PlateNumber = courier.PlateNumber;
                    courierInDb.PhoneNumber = courier.PhoneNumber;
                    courierInDb.CompanyName = courier.CompanyName;
                    _entityLogic.Update(courierInDb);
                    _entityLogic.Save();

                    new Updates().OrdersUpdate();

                    Trace.TraceInformation($"Courier:{JsonConvert.SerializeObject(courierInDb)}");

                    genericResponse = new Response().GenerateResponse(true, $"Successfully updated Courier", null);

                    Trace.TraceInformation($"Courier Updated");

                    return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                }

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Courier - No such Courier exist", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Courier", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }


        // DELETE api/Courier/DeleteCourier
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/Courier/DeleteCourier")]
        public HttpResponseMessage DeleteCourier(Index indexes)
        {
            var genericResponse = new GenericResponse();

            try
            {
                foreach (var id in indexes.Ids)
                {
                    var courier = _entityLogic.GetSingle(c => c.Id == id.Id);

                    if (courier != null)
                    {
                        _entityLogic.Delete(courier);
                        _entityLogic.Save();
                        
                    }
                    else
                    {
                        genericResponse = new Response().GenerateResponse(false, $"No courier exists with id {id} ", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }

                }
                new Updates().CouriersUpdate();
                genericResponse = new Response().GenerateResponse(true, $"Successfully deleted couriers ", null);

                Trace.TraceInformation($"Couriers deleted");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
               

            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting couriers", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }


        public bool ValidateCourier(Courier courier, Actions action)
        {
            var error = "";
            //VALIDATE COURIER
            if (action == Actions.Create)
            {
                var isCourier = _entityLogic.GetSingle(c => c.CompanyName == courier.CompanyName);
                if (isCourier != null)
                {
                    _errorMessage = _errorMessage + "Courier already exists with that name";
                }
            }
            error = error + new Validations<Courier>().ValidateData(courier);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }

    }
}