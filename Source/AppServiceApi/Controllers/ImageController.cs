using AppServiceApi.Util.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppServiceApi.Controllers
{
    public class ImageController : ApiController
    {
        APIManager apiManager = new APIManager();

        // GET: api/Image
        [Route("api/Image")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Image/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]  
        public HttpResponseMessage ImageProcessing([FromBody]AppServiceApi.Models.ApiInput apiInput)
        {
            try
            {
                AppServiceApi.Models.AppraisalOutput appraisalOutput = apiManager.processImageLatLon(getImageAndConvertbase64(), apiInput.latitude, apiInput.longitude);
               // AppServiceApi.Models.AppraisalOutput appraisalOutput = apiManager.processImageLatLon(apiInput.imageBase64, apiInput.latitude, apiInput.longitude);
                return Request.CreateResponse(HttpStatusCode.OK, appraisalOutput);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, " Error ");
            }

        }


        //[HttpPost]        
        //public void Test()
        //{
        //    try
        //    {
        //        string result = apiInput.imageBase64;
        //       // return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //      //  return Request.CreateResponse(HttpStatusCode.BadRequest, " Error ");
        //    }

        //}


        private string getImageAndConvertbase64()
        {
            //string path = @"D:\Workspaces\AppServiceApi\AppServiceApi\Source\AppServiceApi\Resources\Images\Commercial1.jpg";
            string path = @"D:\Workspaces\AppServiceApi\AppServiceApi\Source\AppServiceApi\Resources\Images\Comercial 2.jpg";
            //string path = @"D:\Workspaces\AppServiceApi\AppServiceApi\Source\AppServiceApi\Resources\Images\imagereader4.jpg";
            using (Image image = Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
        }

    }
}
