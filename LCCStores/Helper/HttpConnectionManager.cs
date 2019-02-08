using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace LCCStores.Helper
{
    public sealed class HttpConnectionManager
    {

        private HttpConnectionManager()
        {
            Client = new HttpClient();
        }

        /// <summary>
        /// Singleton Instance of <see cref="HttpConnectionManager"/>
        /// </summary>
        public static HttpConnectionManager Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }
            internal static readonly HttpConnectionManager instance = new HttpConnectionManager();
        }

        public HttpClient Client { get; private set; }

        public void Shutdown()
        {
            if (Client != null)
            {
                Client.CancelPendingRequests();
                Client.Dispose();
                Client = null;
            }
        }


      

    }
}