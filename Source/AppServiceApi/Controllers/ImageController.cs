using AppServiceApi.Util.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppServiceApi.Controllers
{
    public class ImageController : BaseController
    {
        APIManager apiManager;
        string errorMessage;

        // GET: api/Image        
        public IEnumerable<string> GetVersion()
        {              
            return new string[] { "AppService 1.0.0.0" };
        }       

        
        [HttpPost]        
        public HttpResponseMessage ImageProcessing([FromBody]AppServiceApi.Models.ApiInput apiInput)
        {
            try
            {
                if (!IsAuthorised(out errorMessage))
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage);

               apiManager = new APIManager(token);               
               AppServiceApi.Models.AppraisalOutput appraisalOutput = apiManager.processImageLatLon(apiInput.imageBase64, apiInput.latitude, apiInput.longitude);

                return Request.CreateResponse(HttpStatusCode.OK, appraisalOutput);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, " Error ");
            }

        }


        [HttpPost]        
        public HttpResponseMessage AppraiseProperty([FromBody]AppServiceApi.Models.DetailInput detailInput)
        {
            try
            {
                if (!IsAuthorised(out errorMessage))
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage);

                apiManager = new APIManager(token); 
                AppServiceApi.Models.AppraisalOutput appraisalOutput = apiManager.processDetailInput(detailInput);
                return Request.CreateResponse(HttpStatusCode.OK, appraisalOutput);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, " Error ");
            }

        }
        

    }
}
