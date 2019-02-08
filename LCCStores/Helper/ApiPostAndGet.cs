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
    public class ApiPostAndGet
    {
        
        public string UrlGet(string url, string parameters)
        {
            string result = "";

            try
            {


                Trace.TraceInformation($"Getting  URL {url}");
                var http = HttpConnectionManager.Instance;
                using (var resp = http.Client.GetAsync(url).Result)
                {
                    if (!resp.IsSuccessStatusCode) throw new Exception(resp.Content.ReadAsStringAsync().Result);

                    Trace.TraceInformation($"Getting  URL {url} Status: Successful");

                    result = resp.Content.ReadAsStringAsync().Result;

                }

                Trace.TraceInformation($"Done Getting from URL {url}");

                return result;

            }
            catch (Exception ex)
            {
                Trace.TraceInformation($"An Error Occured on Get Request {ex.Message} \n {ex.StackTrace} ");
                throw ex;
            }

        }

        public string UrlPost(string url, object theObject, string bearerToken = null)
        {
            try
            {

                string result = string.Empty;
                var http = HttpConnectionManager.Instance;
                string obj = JsonConvert.SerializeObject(theObject);
                Trace.TraceInformation($"@Request {obj} ");
                StringContent content = new StringContent(obj, Encoding.UTF8, "application/json");
                if (!String.IsNullOrWhiteSpace(bearerToken))
                {
                    http.Client.DefaultRequestHeaders.Remove("Authorization");
                    http.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                }
                using (var resp = http.Client.PostAsync(url, content).Result)
                {
                    if (!resp.IsSuccessStatusCode) throw new Exception(resp.Content.ReadAsStringAsync().Result);


                    result = resp.Content.ReadAsStringAsync().Result;

                }

                Trace.TraceInformation($"Done Posting   URL {url}");

                return result;

            }
            catch (Exception ex)
            {
                Trace.TraceInformation($"An Error Occured on Post Request {ex.Message} \n {ex.StackTrace} ");
                return null;

                //throw ex;
            }

        }



    }
}