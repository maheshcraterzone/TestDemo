using Caching_Demo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;

namespace Caching_Demo.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        private List<BillingPlanResponseModel> GetPlans(int appId)
        {
            List<BillingPlanResponseModel> planList = null;
            string url = "https://gbilling.mobimedia.com.au/AppPlans/" + appId + '/' + "Craterzone";
            object response = Get(url);
            if (response != null)
            {
                planList = JsonConvert.DeserializeObject<List<BillingPlanResponseModel>>(response.ToString());
                planList.Reverse();
            }
            return planList;
        }

        private object Get(string requestUrl)
        {
            try
            {
                //Create http request url
                HttpWebRequest httprequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                //add authentication token
                httprequest.Method = "GET";// "GET";
                httprequest.ContentType = "application/json";
                //encode request into byte array
                //Get response
                using (HttpWebResponse response = httprequest.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return response.StatusCode;
                    }
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    //Read Response
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    //Return response
                    return strsb;
                }
            }
            catch (Exception ex)
            {
                //logger.Error("Exception in calling plan api : " + ex);
                return null;
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            var cache = MemoryCache.Default;
            if (cache.Get(id.ToString()) == null)
            {
                var cachePolicty = new CacheItemPolicy();
               

                var data = GetPlans(id);
                cache.Add(id.ToString(), data, cachePolicty); 
            }
            else
            {
                var data = cache.Get(id.ToString());
            }

            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            var cache = MemoryCache.Default;
            cache.Contains(id.ToString());
            if (cache.Get(id.ToString()) != null)
            {
                cache.Remove(id.ToString());
            }
        }
    }
}
