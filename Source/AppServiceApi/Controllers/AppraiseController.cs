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
    public class AppraiseController : BaseController
    {
        APIManager apiManager;
        string errorMessage;

        // GET: api/Image 
        [Route("getVersion")]
        public HttpResponseMessage GetVersion()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "AppService 1.0.0.0" });            
        }       

        
        [HttpPost]
        [Route("v1/register")]
        public HttpResponseMessage Register([FromBody]Register register)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
       
                return Request.CreateResponse(HttpStatusCode.OK,"Success");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Fail");
            }
        }

        [HttpPost]
        [Route("v1/defaultAppraisal")]
        public HttpResponseMessage ImageProcessing([FromBody]ApiInput apiInput)
        {
            try
            {               
                
                //if (!IsAuthorised(out errorMessage))
                //    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage);

                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);                

                apiManager = new APIManager(token);
                AppServiceApi.Models.AppraisalOutput appraisalOutput = apiManager.processImageLatLon(apiInput.imageBase64, apiInput.latitude, apiInput.longitude, apiInput.deviceId);

                return Request.CreateResponse(HttpStatusCode.OK, appraisalOutput);
            }
            catch (Exception ex)
            {
                ErrorAsync(ex, Request.RequestUri.AbsoluteUri.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Bad Request" });
            } 

        }

        [HttpPost]
        [Route("v1/appraise")]
        public HttpResponseMessage AppraiseProperty([FromBody]DetailInput detailInput)
        {
            try
            {
                //if (!IsAuthorised(out errorMessage))
                //    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage);

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

        [HttpPost]
        [Route("v1/defaultOfferedRentAppraisal")]
        public HttpResponseMessage ImageProcessingForRent([FromBody]OfferedRentApiInput apiInput)
        {
            try
            {
                //if (!IsAuthorised(out errorMessage))
                //    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage);

                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                apiManager = new APIManager(token);
                AppServiceApi.Models.OfferedRentOutput offeredRentOutput = apiManager.processImageLatLonForOfferedRent(apiInput.imageBase64, apiInput.lat, apiInput.lng, apiInput.deviceId);

                return Request.CreateResponse(HttpStatusCode.OK, offeredRentOutput);
            }

            catch (Exception ex)
            {
                ErrorAsync(ex, Request.RequestUri.AbsoluteUri.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Bad Request" });
            }
        }

        [HttpPost]
        [Route("v1/OfferedRentAppraisal")]
        public HttpResponseMessage OfferedRentAppraiseProperty([FromBody]OfferedRentInput offeredRentInput)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                apiManager = new APIManager(token);
                AppServiceApi.Models.OfferedRentOutput offeredRentOutput = apiManager.processOfferedRentInput(offeredRentInput);

                return Request.CreateResponse(HttpStatusCode.OK, offeredRentOutput);
            }
            catch (Exception ex)
            {
                ErrorAsync(ex, Request.RequestUri.AbsoluteUri.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Bad Request" });
            }
        }

        [HttpPost]
        [Route("v1/nearestNeighbour/rentFinancials")]
        public HttpResponseMessage NearestNeighbourRentFinancials([FromBody]RentFinancials rentFinancial)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                apiManager = new APIManager(token);
                object result = apiManager.calculateRentFinancial(rentFinancial);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorAsync(ex, Request.RequestUri.AbsoluteUri.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Bad Request" });
            }
        }

        [HttpPost]
        [Route("v1/nearestNeighbour/rentContracts")]
        public HttpResponseMessage NearestNeighbourRentContracts([FromBody]RentContracts rentContracts)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                apiManager = new APIManager(token);
                object result = apiManager.calculateRentContracts(rentContracts);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorAsync(ex, Request.RequestUri.AbsoluteUri.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Bad Request" });
            }
        }

    }
}
