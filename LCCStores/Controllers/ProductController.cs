using AutoMapper;
using LCCStores.DTO;
using LCCStores.Helper;
using LCCStores.Logic;
using LCCStores.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LCCStores.Controllers
{
    public class ProductController : ApiController
    {

        EntityLogic<Product> _entityLogic;
        EntityLogic<ProductImage> _entityLogicImage;
        EntityLogic<ProductDetail> _entityLogicDetail;

        public ProductController()
        {
            _entityLogic = new EntityLogic<Product>();
            _entityLogicImage = new EntityLogic<ProductImage>();
            _entityLogicDetail = new EntityLogic<ProductDetail>();

        }
        public class RTotalProducts
        {
            public List<Product> Products { get; set; }
            public List<ProductImage> ProductImages { get; set; }
        }
        public class TotalProducts
        {
            public List<ProductDto> Products { get; set; }
            public List<ProductImage> ProductImages { get; set; }
        }
        public class RTotalProduct
        {
            public Product Product { get; set; }
            public List<ProductImage> ProductImages { get; set; }
        }
        public class TotalProduct
        {
            public ProductDto Product { get; set; }
            public List<ProductImage> ProductImages { get; set; }
        }
        // GET api/Product/GetAllProducts
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Product/GetAllProducts")]
        public HttpResponseMessage GetAllProducts()
        {
            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Getting all Products");

                var allProducts = _entityLogic.GetAll(c => c.Brand, c => c.ProductDetail, c => c.Tax, c => c.AddedBy);
                var allProductImages = _entityLogicImage.GetAll();
                var totalProduct = new RTotalProducts()
                {
                    Products = allProducts,
                    ProductImages = allProductImages
                };
                genericResponse = new Response().GenerateResponse(true, "Successfully gotten all products", totalProduct);

                Trace.TraceInformation("Sending all Products");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting products", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }

        }

        // GET api/Product/GetProduct/2
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Product/GetProduct")]
        public HttpResponseMessage GetProduct(int id)
        {
            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation($"Getting Product with ID :{id}");

                var product = _entityLogic.GetSingle(c => c.Id == id, c => c.Brand, c => c.ProductDetail, c => c.Tax, c => c.AddedBy);

                Trace.TraceInformation($"Getting Product with ID :{product.ToString()}");

                var productImage = _entityLogicImage.GetSet(c => c.ProductId == id);
                var totalProduct = new RTotalProduct()
                {
                    Product = product,
                    ProductImages = productImage
                };
                genericResponse = new Response().GenerateResponse(true, $"Successfully got Product:{product.ProductDetail.Name.ToUpper()}", totalProduct);

                Trace.TraceInformation($"Sending Product with ID :{id}");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting products", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Product/CreateProduct")]
        public HttpResponseMessage CreateProduct(TotalProduct totalProduct)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Saving Product");

                //MAPPING PRODUCTDTO TO PRODUCT
                var product = new Product();
                product = Mapper.Map<Product>(totalProduct.Product);              
                product.DateCreated = DateTime.Now;
                product.DateUpdated = DateTime.Now;

                //VALIDATING PRODUCT DETAILS
                if (!ValidateProduct(product)) {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Product:{totalProduct?.Product?.ProductDetail?.Name}- Name already exists", null);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //PROCESSING IMAGES TO SAVE TO SERVER AND DB
                // ProcessImages(totalProduct.ProductImages,product.Brand?.Name);
                if (ProcessImages(totalProduct.ProductImages,product.ProductDetail.Name, "HP")){
                    //SAVE IMAGES TO DB
                    _entityLogic.Insert(product);
                    _entityLogic.Save();
                    try
                    {
                        Trace.TraceInformation($"SAVING IMAGES TO DB, COUNT:{totalProduct.ProductImages.Count}");
                        foreach (var image in totalProduct.ProductImages)
                        {
                            image.ProductId = product.Id;
                            image.Name = $"LCCStores_{ product.Brand.Name.Replace(" ", "_").ToUpper()}_{ DateTime.Now.Ticks}";
                            image.Picture = $"assets/{totalProduct.Product.ProductDetail.Name.ToUpper()}/{image.Name}.jpg";
                            _entityLogicImage.Insert(image);
                        }
                        _entityLogicImage.Save();
                    }
                    catch(Exception e)
                    {
                        Trace.TraceInformation("AN ERROR WAS CAUGHT");
                        Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                        genericResponse = new Response().GenerateResponse(false, $"An error occured while saving Image for Product:{totalProduct?.Product?.ProductDetail?.Name} to DB", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }
                    

                    Trace.TraceInformation($"PRODUCT:{JsonConvert.SerializeObject(product)}");

                    genericResponse = new Response().GenerateResponse(true, $"Successfully added Product:{product?.ProductDetail?.Name?.ToString()}", null);

                    Trace.TraceInformation($"Product:{product?.ProductDetail?.Name?.ToUpper().ToString()} Added");

                    return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                };
                
                genericResponse = new Response().GenerateResponse(false, $"Error saving Images for Product:{product?.ProductDetail?.Name?.ToString()}to server ", null);
                
                return Request.CreateErrorResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Product:{totalProduct?.Product?.ProductDetail?.Name}", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

       public bool ValidateProduct(Product product)
        {
            var isProduct = _entityLogicDetail.GetSingle(c => c.Name.Equals( product.ProductDetail.Name));
            if(isProduct!=null)
            {
                return false;
            }
            return true;
        }

        public bool ProcessImages(List<ProductImage> pictures,string productName, string brand)
        {
            
            foreach (var picture in pictures)
            {
                var pictureName = $"LCCStores_{ productName.Replace(" ", "_").ToUpper()}_{DateTime.Now.Ticks}";
                var image = LoadImage(picture.Picture,pictureName,brand);
            }
            return true;

        }
        public bool LoadImage(string picture,string name,string brand)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            //byte[] bytes = Convert.FromBase64String(picture);

            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    image = Image.FromStream(ms);
            //}

            //return image;
            try
            {
                string filePath = $"C:/Path/{brand.ToUpper()}/{name}.jpg";
               
                File.WriteAllBytes(filePath, Convert.FromBase64String(picture));
                return true;
            }
            catch(Exception e)
            {
                return false;
            }

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