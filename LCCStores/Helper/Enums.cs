using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Helper
{
    public class Enums
    {

    }
    public enum StatusCode
    {
        Success = 0,
        Failure = 1
    }
    public enum Actions
    {
        View=0,
        Create = 1,
        Edit = 2,
        Delete=3
       
    }
}