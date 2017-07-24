using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AppServiceApi.Util.Helper
{
    public class IAZIClientSync
    {        
        HttpWebRequest httpWReq;           

        public string getApiResponse(string url , string token)
        {            
            httpWReq = (HttpWebRequest)WebRequest.Create(url);
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.Headers.Add(HttpRequestHeader.Authorization, token);
            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();

        }


        public string postApiRequest(string url, string postData,string token)
        {
            httpWReq = (HttpWebRequest)WebRequest.Create(url);

            byte[] data = Encoding.UTF8.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.Accept = "application/json";
            httpWReq.ContentType = "application/json";
            httpWReq.ContentLength = data.Length;

            if(!String.IsNullOrEmpty(token))
            httpWReq.Headers.Add(HttpRequestHeader.Authorization, token);

            using (Stream stream = httpWReq.GetRequestStream())
                stream.Write(data, 0, data.Length);

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }


       







    }
}