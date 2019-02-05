using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace LCCStores.Helper
{
    public class Cacher
    {
        public long expireInMinutes = 0;
        public Cacher()
        {
            expireInMinutes = 60 * 24;
        }
        public bool InsertIntoCache(string key, object result)
        {
            try
            {
                HttpRuntime.Cache.Insert(key, result, null, DateTime.Now.AddMinutes(expireInMinutes), Cache.NoSlidingExpiration);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public object GetCache(string key)
        {
            return HttpRuntime.Cache[key];
        }
    }
}