using LCCStores.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface IGenericResponse
    {
        StatusCode Status { get; set; }
        string Message { get; set; }
        bool IsSuccessful { get; set; }
        bool HasData { get; set; }
        object Data { get; set; }
    }
    
}

