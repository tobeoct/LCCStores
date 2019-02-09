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
    public class ReviewController : ApiController
    {


        EntityLogic<Review> _entityLogic;
        EntityLogic<ReviewDetail> _entityLogicDetail;

        string _errorMessage = "";

        public ReviewController()
        {
            _entityLogic = new EntityLogic<Review>();
            _entityLogicDetail = new EntityLogic<ReviewDetail>();
        }
        public class TotalReviews
        {
            public List<Review> Reviews { get; set; }
            public List<ReviewDetail> ReviewDetails { get; set; }
        }
        public class TReview
        {
            public Review Review { get; set; }
            public ReviewDetail ReviewDetail { get; set; }
        }

        // GET api/Review/GetReviews/2
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Review/GetReviews")]
        public HttpResponseMessage GetReviews(int productId)
        {
            var genericResponse = new GenericResponse();
            var reviewsKey = $"TotalReviews{productId.ToString()}";
            var totalReviews = new TotalReviews();
            try
            {

                Trace.TraceInformation($"Getting Reviews ");
                var updateTime = new EntityLogic<ReviewUpdate>().GetSingle(c => c.Id == 1)?.LastUpdateTime;
                if (updateTime != null)
                {
                    if (updateTime < DateTime.Now)
                    {

                        totalReviews = (TotalReviews)new Cacher().GetCache(reviewsKey);
                        if (totalReviews != null)
                        {
                            genericResponse = new Response().GenerateResponse(true, "Successfully gotten reviews", totalReviews);

                            Trace.TraceInformation("Sending all reviews");

                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
                        }

                    }

                }

                var reviews = _entityLogic.GetWhere(c => c.ProductId == productId);
                totalReviews = new TotalReviews();
                if (reviews.Count > 0 && reviews != null)
                {
                    foreach (var review in reviews)
                    {
                        var reviewDetails = _entityLogicDetail.GetWhere(c => c.ReviewId == review.Id);
                        if (reviewDetails != null)
                        {
                            totalReviews.ReviewDetails = reviewDetails;
                        }
                    }
                    totalReviews.Reviews = reviews;
                }
                

                Trace.TraceInformation($"Gotten Reviews :{JsonConvert.SerializeObject(totalReviews)}");

                genericResponse = new Response().GenerateResponse(true, $"Successfully gotten reviews", totalReviews);

                Trace.TraceInformation($"Sending Reviews ");
                new Cacher().InsertIntoCache(reviewsKey, totalReviews);
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));
            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, "An error occured while getting reviews", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }


        // POST api/Review/AddReview
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Review/AddReview")]
        public HttpResponseMessage AddReview(TReview fullReview)
        {

            var genericResponse = new GenericResponse();
            try
            {

                Trace.TraceInformation("Saving Review");

                //VALIDATING REVIEW 
                ValidateReview(fullReview.Review, Actions.Create);
                ValidateReviewDetail(fullReview.ReviewDetail, Actions.Create);

                if (!String.IsNullOrEmpty(_errorMessage))
                {

                    genericResponse = new Response().GenerateResponse(false, $"An error occured while adding Review - {_errorMessage}", null);

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                };

                //SAVE REVIEW TO DB
                Trace.TraceInformation("Saving Review to DB");
                var reviewInDb = _entityLogic.GetWhere(c => c.UserProfileId == fullReview.Review.UserProfileId && c.ProductId == fullReview.Review.ProductId);
                var review = fullReview.Review;
                if (reviewInDb==null || reviewInDb.Count==0)
                {
                    review.Date = DateTime.Now;
                    _entityLogic.Insert(review);
                    _entityLogic.Save();
                }
               
               
                fullReview.ReviewDetail.ReviewId = review.Id;
                fullReview.ReviewDetail.Date = DateTime.Now;
                _entityLogicDetail.Insert(fullReview.ReviewDetail);
                _entityLogicDetail.Save();
               

                new Updates().ReviewsUpdate();


                Trace.TraceInformation($"Review :{JsonConvert.SerializeObject(fullReview)} added Successfully");
                genericResponse = new Response().GenerateResponse(true, $" Review added successfully", null);

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));


            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while adding review", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        // DELETE api/Review/DeleteReview
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/Review/DeleteReview")]
        public HttpResponseMessage DeleteReview(Index indexes)
        {
            var genericResponse = new GenericResponse();

            try
            {

                foreach (var id in indexes.Ids)
                {
                    var review = _entityLogicDetail.GetSingle(c => c.Id == id.Id);

                    if (review != null)
                    {                       
                        _entityLogicDetail.Delete(review);                       
                        _entityLogicDetail.Save();
                    }
                    else
                    {
                        genericResponse = new Response().GenerateResponse(false, $"Review with Id: {id.Id} doesnt exist in the db ", null);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
                    }
                }

                new Updates().CartsUpdate();
                genericResponse = new Response().GenerateResponse(true, $"Successfully removed review ", null);

                Trace.TraceInformation($"Review deleted");

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(genericResponse));



            }
            catch (Exception e)
            {

                Trace.TraceInformation("AN ERROR WAS CAUGHT");
                Trace.TraceInformation($"STACK TRACE:{e.StackTrace.ToString()}, INNER EXCEPTION:{e.InnerException?.Message}, MESSAGE:{e.Message.ToString()}");

                genericResponse = new Response().GenerateResponse(false, $"An error occured while deleting review", null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(genericResponse));
            }
        }

        public bool ValidateReview(Review review, Actions action)
        {
            var error = "";
            //VALIDATE REVIEW
            if (action == Actions.Create)
            {
                var isReview = _entityLogic.GetSingle(c => c.UserProfileId == review.UserProfileId);
                if (isReview != null)
                {
                    error = error + "-User already has a review-";
                }
            }
            error = error + new Validations<Review>().ValidateData(review);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }
        public bool ValidateReviewDetail(ReviewDetail reviewDetail, Actions action)
        {
            var error = "";
            //VALIDATE REVIEWDETAIL
            
            error = error + new Validations<ReviewDetail>().ValidateData(reviewDetail);

            if (!String.IsNullOrEmpty(error))
            {
                _errorMessage = error;
                return false;
            }
            return true;
        }


    }
}