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
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace LCCStores.Controllers
{
    public class ProductController : ApiController
    {

        EntityLogic<Product> _entityLogic;
        EntityLogic<ProductImage> _entityLogicImage;
        EntityLogic<ProductDetail> _entityLogicDetail;
        string _errorMessage = "";

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
            var productKey = $"TotalProducts";

            var totalProduct = new RTotalProducts();
            try
            {

                Trace.TraceInformation("Getting all Products");

                var updateTime = new EntityLogic<ProductUpdate>().GetSingle(c => c.Id == 1).LastUpdateTime;
                if (updateTime < DateTime.Now)
                {
                    totalProduct = (RTotalProducts)new Cacher().GetCache(productKey);
                    if (totalProduct != null)
                    {
                        genericResponse = new Response().GenerateResponse(true, "Successfully gotten all products", totalProduct);

                        Trace.TraceInformation("Sending all Products");

                        return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                    }
                }

                var allProducts = _entityLogic.GetAll(c => c.Brand, c => c.ProductDetail, c => c.Tax, c => c.AddedBy).OrderByDescending(c => c.DateCreated).ToList();
                var allProductImages = _entityLogicImage.GetAll();

                totalProduct = new RTotalProducts()
                {
                    Products = allProducts,
                    ProductImages = allProductImages
                };

                genericResponse = new Response().GenerateResponse(true, "Successfully gotten all products", totalProduct);

                Trace.TraceInformation("Sending all Products");

                //Cache Products
                new Cacher().InsertIntoCache(productKey, totalProduct);
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

        // GET api/Product/GetAllProductsByBrand
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Product/GetAllProductsByBrand")]
        public HttpResponseMessage GetAllProductsByBrand(string brand)
        {
            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Getting all Products");

                var allProducts = _entityLogic.GetAll(c => c.Brand, c => c.ProductDetail, c => c.Tax, c => c.AddedBy).OrderByDescending(c => c.DateCreated).ToList();
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

        // GET api/Product/GetProductsByPage
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Product/GetProductsByPage")]
        public HttpResponseMessage GetProductsByPage(int pageNumber, string brand)
        {
            var productKey = $"TotalProducts";

            var totalProduct = new RTotalProducts();
            //var productImageKey = $"ProductImages";

            // var productImagesInCache = (List<Product>)HttpRuntime.Cache[productImageKey];
            var genericResponse = new GenericResponse();
            try
            {


                Trace.TraceInformation("Getting all Products");
                List<Product> allProducts = new List<Product>();
                if (String.IsNullOrEmpty(brand))
                {
                    allProducts = _entityLogic.GetN(pageNumber, null, c => c.Brand, c => c.ProductDetail, c => c.Tax, c => c.AddedBy).Take((pageNumber * 10)).OrderByDescending(c => c.DateCreated).ToList();
                }
                else
                {
                    allProducts = _entityLogic.GetN(pageNumber, c => c.Brand.Name.Equals(brand), c => c.Brand, c => c.ProductDetail, c => c.Tax, c => c.AddedBy).Take((pageNumber * 10)).OrderByDescending(c => c.DateCreated).ToList();

                }
                var allProductImages = _entityLogicImage.GetAll();


                totalProduct.Products = allProducts;
                totalProduct.ProductImages = allProductImages;

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

                var productImage = _entityLogicImage.GetWhere(c => c.ProductId == id);
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
                ValidateProduct(product, Actions.Create);
                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Product:{totalProduct?.Product?.ProductDetail?.Name}- {_errorMessage}", null);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //PROCESSING IMAGES TO SAVE TO SERVER AND DB
                // ProcessImages(totalProduct.ProductImages,product.Brand?.Name);
                if (ProcessImages(totalProduct.ProductImages, product.ProductDetail.Name, "HP"))
                {
                    //SAVE IMAGES TO DB
                    _entityLogic.Insert(product);
                    _entityLogic.Save();
                    try
                    {
                        Trace.TraceInformation($"SAVING IMAGES TO DB, COUNT:{totalProduct.ProductImages.Count}");
                        foreach (var image in totalProduct.ProductImages)
                        {
                            image.ProductId = product.Id;
                            image.Name = $"LCCStores_{ product.ProductDetail.Name.Replace(" ", "_").ToUpper()}_{ DateTime.Now.Ticks}";
                            image.Picture = $"assets/{totalProduct.Product.ProductDetail.Name.ToUpper()}/{image.Name}.jpg";
                            _entityLogicImage.Insert(image);
                        }
                        _entityLogicImage.Save();

                        new Updates().ProductsUpdate();

                    }
                    catch (Exception e)
                    {
                        Trace.TraceInformation("AN ERROR WAS CAUGHT");
                        Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                        genericResponse = new Response().GenerateResponse(false, $"An error occured while saving Image for Product:{totalProduct?.Product?.ProductDetail?.Name} to DB: {_errorMessage}", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }


                    Trace.TraceInformation($"PRODUCT:{JsonConvert.SerializeObject(product)}");

                    genericResponse = new Response().GenerateResponse(true, $"Successfully added Product:{product?.ProductDetail?.Name?.ToString()}", null);

                    Trace.TraceInformation($"Product:{product?.ProductDetail?.Name?.ToUpper().ToString()} Added");

                    return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                };

                genericResponse = new Response().GenerateResponse(false, $"Error saving Images for Product:{product?.ProductDetail?.Name?.ToString()}to server {_errorMessage}", null);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Product:{totalProduct?.Product?.ProductDetail?.Name}", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // POST api/<controller>
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/Product/EditProduct")]
        public HttpResponseMessage EditProduct(TotalProduct totalProduct)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("UPDATING Product");

                //MAPPING PRODUCTDTO TO PRODUCT
                var product = new Product();
                product = Mapper.Map<Product>(totalProduct.Product);
                product.DateCreated = DateTime.Now;
                product.DateUpdated = DateTime.Now;

                //VALIDATING PRODUCT DETAILS                
                ValidateProduct(product, Actions.Edit);
                if (!String.IsNullOrEmpty(_errorMessage))
                {
                    genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Product:{totalProduct?.Product?.ProductDetail?.Name}- {_errorMessage}", null);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                _entityLogic.Update(product);
                _entityLogicDetail.Update(totalProduct.Product.ProductDetail);
                _entityLogic.Save();

                //PROCESSING IMAGES TO SAVE TO SERVER AND DB
                // ProcessImages(totalProduct.ProductImages,product.Brand?.Name);
                if (totalProduct.ProductImages != null || totalProduct.ProductImages.Count > 0)
                {

                    if (ProcessImages(totalProduct.ProductImages, product.ProductDetail.Name, "HP"))
                    {
                        //SAVE IMAGES TO DB
                        var productImagesInDb = _entityLogicImage.GetWhere(c => c.ProductId == product.Id);
                        try
                        {
                            Trace.TraceInformation($"UPDATING IMAGES IN DB, COUNT:{totalProduct.ProductImages.Count}");
                            foreach (var image in totalProduct.ProductImages)
                            {
                                var productImageInDB = productImagesInDb.Where(c => c.Order == image.Order)?.FirstOrDefault();
                                image.Id = productImageInDB.Id;
                                image.Name = $"LCCStores_{totalProduct.Product.ProductDetail.Name}_{ DateTime.Now.Hour}";
                                image.Picture = $"assets/{totalProduct.Product.ProductDetail.Name.ToUpper()}/{image.Name}.jpg";
                                image.ProductId = totalProduct.Product.Id;
                                _entityLogicImage.Update(image);
                            }
                            _entityLogicImage.Save();

                            new Updates().ProductsUpdate();

                        }
                        catch (Exception e)
                        {
                            Trace.TraceInformation("AN ERROR WAS CAUGHT");
                            Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                            genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Image for Product:{totalProduct?.Product?.ProductDetail?.Name} to DB", null);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                        }


                        Trace.TraceInformation($"PRODUCT:{JsonConvert.SerializeObject(product)}");

                        genericResponse = new Response().GenerateResponse(true, $"Successfully updated Product:{product?.ProductDetail?.Name?.ToString()}", null);

                        Trace.TraceInformation($"Product:{product?.ProductDetail?.Name?.ToUpper().ToString()} Added");

                        return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                    };
                }
                genericResponse = new Response().GenerateResponse(false, $"Error updating Images for Product:{product?.ProductDetail?.Name?.ToString()}to server ", null);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while updating Product:{totalProduct?.Product?.ProductDetail?.Name}", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }
      
        // DELETE api/Product/DeleteProduct/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/Product/DeleteProduct")]
        public HttpResponseMessage DeleteProduct(Index indexes)
        {
            var genericResponse = new GenericResponse();
            var products = "";
            try
            {
                foreach (var id in indexes.Ids)
                {
                    var product = _entityLogic.GetSingle(c => c.Id == id.Id, c => c.ProductDetail);
                    if (product != null)
                    {
                        products = products + $"-{product?.ProductDetail?.Name}-";
                        var productDetail = _entityLogicDetail.GetSingle(c => c.Id == product?.ProductDetailId);
                        var productImages = _entityLogicImage.GetWhere(c => c.ProductId == id.Id);
                        if (productImages != null)
                        {

                            foreach (var image in productImages)
                            {
                                _entityLogicImage.Delete(image);
                            }
                            _entityLogic.Delete(product);
                            _entityLogicDetail.Delete(productDetail);
                        }
                    }
                    else
                    {
                        genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting product - No such product exists", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }
                }
                _entityLogic.Save();
                new Updates().BrandsUpdate();
                genericResponse = new Response().GenerateResponse(true, $"Successfully deleted {products}", null);

                Trace.TraceInformation($"Products:{products.ToUpper()} deleted");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));



            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting product", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }

        }

        public bool ValidateProduct(Product product, Actions action)
        {
            var error = "";
            //VALIDATE PRODUCT
            if (action == Actions.Create)
            {
                var isProduct = _entityLogicDetail.GetSingle(c => c.Name.Equals(product.ProductDetail.Name));
                if (isProduct != null)
                {
                    error = error + "-Name already exists-";
                }
            }
            error = error + new Validations<Product>().ValidateData(product);
            if (product.ProductDetail != null)
            {
                error = error + new Validations<ProductDetail>().ValidateData(product.ProductDetail);
            }
            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }

        public bool ProcessImages(List<ProductImage> pictures, string productName, string brand)
        {

            foreach (var picture in pictures)
            {
                var pictureName = $"LCCStores_{ productName.Replace(" ", "_").ToUpper()}_{DateTime.Now.Hour}";
                var image = LoadImage(picture.Picture, pictureName, brand);
            }
            return true;

        }

        public bool LoadImage(string picture, string name, string brand)
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
            catch (Exception e)
            {
                return false;
            }

        }

       
    }
}