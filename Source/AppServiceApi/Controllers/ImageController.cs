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
using Swashbuckle.Swagger.Annotations;
using log4net.Repository.Hierarchy;
using log4net;
using AppServiceApi.Util.Log;
using System.Diagnostics;
using AppServiceApi.Models;

namespace AppServiceApi.Controllers
{
    public class ImageController : BaseController
    {
        APIManager apiManager;
        string errorMessage;

        // GET: api/Image        
        public HttpResponseMessage GetVersion()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "AppService 1.0.0.0" });            
        }       

        
        [HttpPost]        
        public HttpResponseMessage ImageProcessing([FromBody]ApiInput apiInput)
        {
            try
            {               
                
                if (!IsAuthorised(out errorMessage))
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage);

                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);                

                apiManager = new APIManager(token);
                AppServiceApi.Models.AppraisalOutput appraisalOutput = apiManager.processImageLatLon(apiInput.imageBase64, apiInput.latitude, apiInput.longitude);

                return Request.CreateResponse(HttpStatusCode.OK, appraisalOutput);
            }
            catch (Exception ex)
            {
                ErrorAsync(ex, Request.RequestUri.AbsoluteUri.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Bad Request" });
            } 

        }


        [HttpPost] 
       
        public HttpResponseMessage AppraiseProperty([FromBody]DetailInput detailInput)
        {
            try
            {
                if (!IsAuthorised(out errorMessage))
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage);

                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                apiManager = new APIManager(token); 
                AppServiceApi.Models.AppraisalOutput appraisalOutput = apiManager.processDetailInput(detailInput);
                return Request.CreateResponse(HttpStatusCode.OK, appraisalOutput);
            }
            catch (Exception ex)
            {
                ErrorAsync(ex, Request.RequestUri.AbsoluteUri.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Bad Request" });
            }

        }
        

    }
}
