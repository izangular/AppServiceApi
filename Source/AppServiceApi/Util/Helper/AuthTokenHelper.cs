using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AppServiceApi.Util.Helper
{
    public class AuthTokenHelper
    {
        #region AV Service Token Generation
        /// <summary>
        /// Street Service Token generation
        /// </summary>
        /// <returns></returns>
        public static string GetAuthToken()
        {
            try
            {
                string postData = "userEmail=" + ConfigurationManager.AppSettings["username"] + "&userPwd=" + ConfigurationManager.AppSettings["password"]
                                  + "&app=" + ConfigurationManager.AppSettings["app"];

                string tokenResponse = getTokenResponse(ConfigurationManager.AppSettings["Server"] + ConfigurationManager.AppSettings["TokenService"], postData);
                string token = ExtractTokenFromResponse(tokenResponse);

                if (string.IsNullOrEmpty(token))
                { /* (1) spr */
                    string message = ". Empty Token generated from Token Service " + Convert.ToString(ConfigurationManager.AppSettings["TokenService"]);

                    var customex = new Exception(message + "    Response:" + tokenResponse); // (1) spr, create meaningful custom exception.
                    // Elmah.ErrorSignal.FromCurrentContext().Raise(customex, System.Web.HttpContext.Current);  /* (1) spr,This will unconditionally log the error with the current default error log and send mail */
                }

                return token;
            }
            catch (Exception ex)
            {
                /*(1) SPR , should not break CLient Application if mail doesn't work.Custome msg set because require more meaning full info while debugging. Since user not logged in */
                string message = ex.Message + ". Exception from Token Service " + Convert.ToString(ConfigurationManager.AppSettings["TokenService"]);

                var customex = new Exception(message, ex.InnerException); // (1) create meaningful custom exception.
                // Elmah.ErrorSignal.FromCurrentContext().Raise(customex, System.Web.HttpContext.Current);  /* (1) spr,This will unconditionally log the error with the current default error log and send mail */

                return "-999";
            }
        }

        private static string getTokenResponse(string url, string postData)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);

            byte[] data = Encoding.UTF8.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
                stream.Write(data, 0, data.Length);

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        private static string ExtractTokenFromResponse(string tokenResponse)
        {
            dynamic stuff = JsonConvert.DeserializeObject(tokenResponse);

            if (stuff != null)
                return stuff.token_type + " " + stuff.token;
            else
                return "";
        }
        #endregion

    }
}