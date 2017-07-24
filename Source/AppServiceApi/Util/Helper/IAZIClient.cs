using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace AppServiceApi.Util.Helper
{
    public class IAZIClient
    {
        protected HttpClient httpClient;

        protected HttpResponseMessage httpResponse;

        public IAZIClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        /// <summary>
        /// Post web api & get response.
        /// </summary>
        /// <param name="oList"></param>
        /// <param name="ServiceLocation"></param>
        /// <returns></returns>
        public async Task PostRequest(Object oList, string ServiceLocation)
        {
            try
            {
                httpResponse = await httpClient.PostAsJsonAsync(ServiceLocation, oList);
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the response from the Api call using the token 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token">Authentication token</param>
        /// <returns>Returns AvResponse</returns>
        public async Task GetResponseAuthenticated(string url)
        {
            try
            {
                httpResponse = await httpClient.GetAsync(url); 
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}