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
    public class CustomerController : ApiController
    {
        EntityLogic<Customer> _entityLogic;
        EntityLogic<BillingInformation> _entityLogicBilling;
        EntityLogic<PersonalInformation> _entityLogicPersonal;
        string _errorMessage = "";

        public CustomerController()
        {
            _entityLogic = new EntityLogic<Customer>();
            _entityLogicPersonal = new EntityLogic<PersonalInformation>();
            _entityLogicBilling = new EntityLogic<BillingInformation>();
        }

        public class RCustomers
        {
            public List<Customer> Customers { get; set; }

        }

        public class Customers
        {
            public List<Customer> TotalCustomers { get; set; }

        }

        // GET api/Customer/GetAllCustomers
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Customer/GetAllCustomers")]
        public HttpResponseMessage GetAllCustomers()
        {
            var genericResponse = new GenericResponse();
            var customersKey = $"TotalCustomers";

            var customers = new RCustomers();
            try
            {

                Trace.TraceInformation("Getting all Customers");

                var updateTime = new EntityLogic<CustomerUpdate>().GetSingle(c => c.Id == 1)?.LastUpdateTime;
                if (updateTime != null)
                {
                    if (updateTime < DateTime.Now)
                    {
                        customers.Customers = (List<Customer>)new Cacher().GetCache(customersKey);
                        if (customers.Customers != null)
                        {
                            genericResponse = new Response().GenerateResponse(true, "Successfully gotten all customers", customers);

                            Trace.TraceInformation("Sending all Customers");

                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                        }
                    }

                }
                var allCustomers = _entityLogic.GetAll(c => c.BillingInfo, c => c.PersonalInfo, c => c.PersonalInfo.PhoneNumber, c => c.PersonalInfo.Address, c => c.PersonalInfo.Address.Country, c => c.PersonalInfo.Address.City)?.OrderByDescending(c => c.Id).ToList();

                genericResponse = new Response().GenerateResponse(true, "Successfully gotten all customers", allCustomers);

                Trace.TraceInformation("Sending all Customers");

                //Cache Products
                new Cacher().InsertIntoCache(customersKey, allCustomers);
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting customers", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }

        }

        // GET api/Customer/GetCustomer/2
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Customer/GetCustomer")]
        public HttpResponseMessage GetCustomer(int id)
        {
            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation($"Getting Customer with ID :{id}");

                var customer = _entityLogic.GetSingle(c => c.Id == id, c => c.BillingInfo, c => c.PersonalInfo);

                Trace.TraceInformation($"Gotten Customer with ID :{JsonConvert.SerializeObject(customer)}");


                genericResponse = new Response().GenerateResponse(true, $"Successfully got Brand:{customer.FirstName.ToUpper()}", customer);

                Trace.TraceInformation($"Sending Customer with ID :{id}");

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

        // POST api/Customer/CreateCustomer
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Customer/CreateCustomer")]
        public HttpResponseMessage CreateCustomer(Customer customer)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Saving Customer");

                //VALIDATING CUSTOMER DETAILS
                ValidateCustomer(customer, Actions.Create);
                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Customer:{customer?.FirstName}- {_errorMessage}", null);

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //SAVE CUSTOMER TO DB
                Trace.TraceInformation("Saving Customer to DB");
                var personalInfo = customer.PersonalInfo;
                customer.BillingInfoId = customer.BillingInfoId;
                if (personalInfo != null)
                {
                    ValidateCustomerPersonalInfo(personalInfo, Actions.Create);
                    ValidateCustomerAddress(personalInfo?.Address, Actions.Create);
                    ValidateCustomerPhonenumber(personalInfo?.PhoneNumber, Actions.Create);
                    if (!String.IsNullOrEmpty(_errorMessage))
                    {

                        genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Customer:{customer?.FirstName}- {_errorMessage}", null);

                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    };
                    if (CheckIfCityExists(customer.PersonalInfo.Address.City.Name) != 0)
                    {
                        customer.PersonalInfo.Address.CityId = CheckIfCityExists(customer.PersonalInfo.Address.City.Name);
                        customer.PersonalInfo.Address.City = null;
                    }
                    if (CheckIfCountryExists(customer.PersonalInfo.Address.Country.Name) != 0)
                    {
                        customer.PersonalInfo.Address.CountryId = CheckIfCountryExists(customer.PersonalInfo.Address.Country.Name);
                        customer.PersonalInfo.Address.Country = null;
                    }
                    _entityLogicPersonal.Insert(personalInfo);
                    customer.PersonalInfoId = personalInfo.Id;
                    customer.PersonalInfo = null;
                }
                var billingInfo = customer.BillingInfo;
                if (billingInfo != null)
                {

                    ValidateCustomerBillingInfo(billingInfo, Actions.Create);
                    ValidateCustomerAddress(billingInfo?.Address, Actions.Create);
                    ValidateCustomerPhonenumber(billingInfo?.PhoneNumber, Actions.Create);
                    if (!String.IsNullOrEmpty(_errorMessage))
                    {

                        genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Customer:{customer?.FirstName}- {_errorMessage}", null);

                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    };

                    _entityLogicBilling.Insert(billingInfo);
                    customer.BillingInfoId = billingInfo.Id;
                    customer.BillingInfo = null;
                }



                _entityLogic.Insert(customer);
                _entityLogic.Save();
                new Updates().CustomersUpdate();


                Trace.TraceInformation($"Customer :{JsonConvert.SerializeObject(customer)} Created Successfully");
                genericResponse = new Response().GenerateResponse(true, $" Customer:{customer?.FirstName} added successfully", null);

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Customer:{customer?.FirstName}", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // PUT api/Customer/EditCustomer
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/Customer/EditCustomer")]
        public HttpResponseMessage EditCustomer(Customer customer)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("UPDATING Customer");

                //VALIDATING PRODUCT DETAILS                
                ValidateCustomer(customer, Actions.Edit);
                if (!String.IsNullOrEmpty(_errorMessage))
                {
                    genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Customer:{customer?.FirstName}-{_errorMessage}", null);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                if (customer.PersonalInfoId != null)
                {

                    ValidateCustomerPersonalInfo(customer.PersonalInfo, Actions.Edit);
                    customer.PersonalInfo.Id = customer.PersonalInfoId;
                    UpdateAddressInDb(customer.PersonalInfo.Address);
                    UpdatePhoneNumberInDb(customer.PersonalInfo.PhoneNumber);
                    if (!String.IsNullOrEmpty(_errorMessage))
                    {
                        genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Customer:{customer?.FirstName}-{_errorMessage}", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    };
                    customer.PersonalInfo.Address = null;
                    customer.PersonalInfo.PhoneNumber = null;
                    _entityLogicPersonal.Update(customer.PersonalInfo);
                }
                var customerInDb = _entityLogic.GetSingle(c => c.Id == customer.Id);
                customerInDb.FirstName = customer.FirstName;
                customerInDb.LastName = customer.LastName;
                customerInDb.PersonalInfo = customer.PersonalInfo;
                _entityLogic.Update(customer);
                _entityLogic.Save();

                new Updates().CustomersUpdate();

                Trace.TraceInformation($"CUSTOMER:{JsonConvert.SerializeObject(customer)}");

                genericResponse = new Response().GenerateResponse(true, $"Successfully updated Customer:{customer?.FirstName?.ToString()}", null);

                Trace.TraceInformation($"CUSTOMER:{customer.FirstName?.ToUpper().ToString()} Updated");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));



            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Customer:{customer?.FirstName}", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // DELETE api/Brand/DeleteBrand/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/Customer/DeleteCustomer")]
        public HttpResponseMessage DeleteCustomer(Index indexes)
        {
            var genericResponse = new GenericResponse();
            var customers = "";
            try
            {
                foreach (var id in indexes.Ids)
                {
                    var customer = _entityLogic.GetSingle(c => c.Id == id.Id);

                    if (customer == null)
                    {
                        genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting Customer - No such Customer exists", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }

                    customers = customers + $"-{customer?.FirstName}-";
                    var personalId = customer.PersonalInfoId;
                    var billingId = customer.BillingInfoId;
                    _entityLogic.Delete(customer);
                    if (personalId != null)
                    {
                        DeletePersonalInfoFromDB(personalId);
                    }
                    if (billingId != null)
                    {
                        DeleteBillingInfoFromDB(billingId);
                    }
                    var cartLogic = new EntityLogic<Cart>();
                    var cartItemLogic = new EntityLogic<CartItem>();
                    var cart = cartLogic.GetSingle(c => c.CustomerId == id.Id);
                    if (cart != null)
                    {
                        var cartItems = cartItemLogic.GetWhere(c => c.CartId == cart?.Id);
                        foreach (var item in cartItems)
                        {
                            cartItemLogic.Delete(item);
                        }
                        cartLogic.Delete(cart);
                    }
                    if (!String.IsNullOrEmpty(_errorMessage))
                    {
                        genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting Customer {_errorMessage}", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }



                }
                _entityLogic.Save();
                new Updates().CustomersUpdate();
                genericResponse = new Response().GenerateResponse(true, $"Successfully deleted {customers}", null);

                Trace.TraceInformation($"Customers:{customers.ToUpper()} deleted");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));



            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting customer", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }

        }

        public int CheckIfCityExists(string city)
        {
            try
            {
                var cityInDb = new EntityLogic<City>().GetSingle(c => c.Name.Equals(city));

                if (cityInDb != null)
                {
                    return cityInDb.Id;
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int CheckIfCountryExists(string country)
        {
            try
            {

                var countryInDb = new EntityLogic<Country>().GetSingle(c => c.Name.Equals(country));
                if (countryInDb != null)
                {
                    return countryInDb.Id;
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public bool DeletePersonalInfoFromDB(int personalInfoId)
        {
            try
            {
                var addresslogic = new EntityLogic<Address>();
                var phonelogic = new EntityLogic<PhoneNumber>();
                var personalInfo = _entityLogicPersonal.GetSingle(c => c.Id == personalInfoId, c => c.Address, c => c.PhoneNumber);

                if (personalInfo == null)
                {
                    _errorMessage = _errorMessage + "-No such personal info exists";
                    return false;
                }
                _entityLogicPersonal.Delete(personalInfo);
                if (personalInfo.Address != null)
                {
                    addresslogic.Delete(personalInfo.Address);
                }
                if (personalInfo.PhoneNumber != null)
                {
                    phonelogic.Delete(personalInfo.PhoneNumber);
                }
                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool DeleteBillingInfoFromDB(int? billingInfoId)
        {
            try
            {
                var addresslogic = new EntityLogic<Address>();
                var phonelogic = new EntityLogic<PhoneNumber>();
                var cardLogic = new EntityLogic<CreditCard>();
                var billingInfo = _entityLogicBilling.GetSingle(c => c.Id == billingInfoId, c => c.Address, c => c.PhoneNumber);

                if (billingInfo == null)
                {
                    _errorMessage = _errorMessage + "-No such personal info exists";
                    return false;
                }
                var cards = cardLogic.GetWhere(c => c.BillingInfoId == billingInfo.Id);
                _entityLogicBilling.Delete(billingInfo);
                if (billingInfo.Address != null)
                {
                    addresslogic.Delete(billingInfo.Address);
                }
                if (billingInfo.PhoneNumber != null)
                {
                    phonelogic.Delete(billingInfo.PhoneNumber);
                }
                foreach (var card in cards)
                {
                    cardLogic.Delete();
                }
                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool UpdateAddressInDb(Address address)
        {
            try
            {
                if (address == null)
                {
                    return false;
                }
                ValidateCustomerAddress(address, Actions.Edit);
                if (!String.IsNullOrEmpty(_errorMessage))
                {
                    return false;
                };
                var logic = new EntityLogic<Address>();
                var addressInDb = logic.GetSingle(c => c.Id == address.Id);
                addressInDb.Street = address.Street;
                addressInDb.CountryId = address.CountryId;
                addressInDb.Country = null;
                addressInDb.CityId = address.CityId;
                logic.Update(addressInDb);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool UpdatePhoneNumberInDb(PhoneNumber phoneNumber)
        {
            try
            {
                if (phoneNumber == null)
                {
                    return false;
                }
                ValidateCustomerPhonenumber(phoneNumber, Actions.Edit);
                if (!String.IsNullOrEmpty(_errorMessage))
                {
                    return false;
                };
                var logic = new EntityLogic<PhoneNumber>();
                var phoneNumberInDb = logic.GetSingle(c => c.Id == phoneNumber.Id);
                phoneNumberInDb.NumberOne = phoneNumber.NumberOne;
                phoneNumberInDb.NumberTwo = phoneNumber.NumberTwo;
                logic.Update(phoneNumberInDb);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool ValidateCustomer(Customer customer, Actions action)
        {
            var error = "";
            //VALIDATE CUSTOMER
            if (action == Actions.Create)
            {
                var isCart = _entityLogic.GetSingle(c => c.PersonalInfoId == customer.PersonalInfoId);
                if (isCart != null)
                {
                    error = error + "-Customer already exists-";
                }
            }
            error = error + new Validations<Customer>().ValidateData(customer);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }
        public bool ValidateCustomerBillingInfo(BillingInformation billingInfo, Actions action)
        {
            var error = "";

            error = error + new Validations<BillingInformation>().ValidateData(billingInfo);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }
        public bool ValidateCustomerPersonalInfo(PersonalInformation personalInfo, Actions action)
        {
            var error = "";

            if (action == Actions.Create)
            {
                var isCustomer = _entityLogicPersonal.GetSingle(c => c.Email.Equals(personalInfo.Email));
                if (isCustomer != null)
                {
                    error = error + "-Customer already exists with that email address-";
                }
            }
            if (action == Actions.Edit)
            {
                var isCustomer = _entityLogicPersonal.GetSingle(c => c.Id == personalInfo.Id);
                if (isCustomer == null)
                {
                    error = error + "-No such Customer exists -";
                }
            }
            error = error + new Validations<PersonalInformation>().ValidateData(personalInfo);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }
        public bool ValidateCustomerAddress(Address address, Actions action)
        {
            var error = "";
            //VALIDATE CUSTOMER

            error = error + new Validations<Address>().ValidateData(address);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }
        public bool ValidateCustomerPhonenumber(PhoneNumber phoneNumber, Actions action)
        {
            var error = "";
            //VALIDATE CUSTOMER

            error = error + new Validations<PhoneNumber>().ValidateData(phoneNumber);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }
    }
}