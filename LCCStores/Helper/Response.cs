using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Helper
{
    public class Response
    {
        GenericResponse response;
        public Response()
        {
            response = new GenericResponse();
        }
        public GenericResponse GenerateResponse(bool isSuccessful, string message, object data)
        {
            response.Data = data;
            response.IsSuccessful = isSuccessful;
            response.Status = StatusCode.Failure;
            if (isSuccessful)
            {
                response.Status = StatusCode.Success;
            }
            response.HasData = false;
            if (data != null)
            {
                response.HasData = true;
            }
            response.Message = message;
            if(String.IsNullOrEmpty(message))
            {
                response.Message = "An error occured";
            }
            return response;

        }
    }
}