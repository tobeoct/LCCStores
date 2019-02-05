using AutoMapper;
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
    public class BrandController : ApiController
    {
        EntityLogic<Brand> _entityLogic;
        string _errorMessage = "";

        public BrandController()
        {
            _entityLogic = new EntityLogic<Brand>();        
        }

        public class RBrands
        {
            public List<Brand> Brands { get; set; }

        }

        public class Brands
        {
            public List<Brand> TotalBrands { get; set; }

        }

        // GET api/Brand/GetAllBrands
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Brand/GetAllBrands")]
        public HttpResponseMessage GetAllBrands()
        {
            var genericResponse = new GenericResponse();
            var productKey = $"TotalBrands";

            var brands = new RBrands();
            try
            {

                Trace.TraceInformation("Getting all Brands");

                var updateTime = new EntityLogic<ProductUpdate>().GetSingle(c => c.Id == 1).LastUpdateTime;
                if (updateTime < DateTime.Now)
                {
                    brands = (RBrands)new Cacher().GetCache(productKey);
                    if (brands != null)
                    {
                        genericResponse = new Response().GenerateResponse(true, "Successfully gotten all brands", brands);

                        Trace.TraceInformation("Sending all Brands");

                        return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                    }
                }

                var allBrands = _entityLogic.GetAll(c => c.AddedBy).OrderByDescending(c => c.Id).ToList();

                genericResponse = new Response().GenerateResponse(true, "Successfully gotten all brands", allBrands);

                Trace.TraceInformation("Sending all Brands");

                //Cache Products
                new Cacher().InsertIntoCache(productKey, allBrands);
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting brands", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }

        }

        // GET api/Brand/GetBrand/2
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Brand/GetBrand")]
        public HttpResponseMessage GetBrand(int id)
        {
            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation($"Getting Brand with ID :{id}");

                var brand = _entityLogic.GetSingle(c => c.Id == id, c => c.AddedBy);

                Trace.TraceInformation($"Getting Brand with ID :{JsonConvert.SerializeObject(brand)}");


                genericResponse = new Response().GenerateResponse(true, $"Successfully got Brand:{brand.Name.ToUpper()}", brand);

                Trace.TraceInformation($"Sending Brand with ID :{id}");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting brands", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/Brand/CreateBrand
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Brand/CreateBrand")]
        public HttpResponseMessage CreateBrand(Brand brand)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Saving Product");

                //VALIDATING PRODUCT DETAILS
                ValidateBrand(brand, Actions.Create);
                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Brand:{brand?.Name}- {_errorMessage}", null);

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //SAVE BRAND TO DB
                Trace.TraceInformation("Saving Product to DB");
                _entityLogic.Insert(brand);
                _entityLogic.Save();
                new Updates().BrandsUpdate();


                Trace.TraceInformation($"Brand :{JsonConvert.SerializeObject(brand)} Created Successfully");
                genericResponse = new Response().GenerateResponse(true, $" Brand:{brand?.Name} added successfully", null);

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Brand:{brand?.Name}", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/Brand/EditBrand
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/Brand/EditBrand")]
        public HttpResponseMessage EditProduct(Brand brand)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("UPDATING Brand");

                //VALIDATING PRODUCT DETAILS                
                ValidateBrand(brand, Actions.Edit);
                if (!String.IsNullOrEmpty(_errorMessage))
                {
                    genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Brand:{brand?.Name}- {_errorMessage}", null);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                _entityLogic.Update(brand);
                _entityLogic.Save();

                new Updates().BrandsUpdate();

                Trace.TraceInformation($"BRAND:{JsonConvert.SerializeObject(brand)}");

                genericResponse = new Response().GenerateResponse(true, $"Successfully updated Brand:{brand?.Name?.ToString()}", null);

                Trace.TraceInformation($"BRAND:{brand.Name?.ToUpper().ToString()} Added");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));



            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Brand:{brand?.Name}", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // DELETE api/Brand/DeleteBrand/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/Brand/DeleteBrand")]
        public HttpResponseMessage DeleteBrand(Index indexes)
        {
            var genericResponse = new GenericResponse();
            var brands = "";
            try
            {
                foreach (var id in indexes.Ids)
                {
                    var brand = _entityLogic.GetSingle(c => c.Id == id.Id, c => c.AddedBy);
                    if (brand == null)
                    {
                        genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting Brand - No such brand exists", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }
                        brands = brands + $"-{brand?.Name}-";
                        _entityLogic.Delete(brand);
                    
                }
                _entityLogic.Save();
                new Updates().BrandsUpdate();
                genericResponse = new Response().GenerateResponse(true, $"Successfully deleted {brands}", null);

                Trace.TraceInformation($"Brands:{brands.ToUpper()} deleted");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));



            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting brands", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }

        }

        public bool ValidateBrand(Brand brand, Actions action)
        {
            var error = "";
            //VALIDATE PRODUCT
            if (action == Actions.Create)
            {
                var isProduct = _entityLogic.GetSingle(c => c.Name.Equals(brand.Name));
                if (isProduct != null)
                {
                    error = error + "-Name already exists-";
                }
            }
            error = error + new Validations<Brand>().ValidateData(brand);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }


    }

}