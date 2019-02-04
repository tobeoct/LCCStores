using LCCStores.Contracts;
using LCCStores.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Models
{
    public class GenericResponse : IGenericResponse
    {
        public StatusCode Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful {get; set; }
        public bool HasData {get; set; }
        public object Data {get; set; }
    }
}