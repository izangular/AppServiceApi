using AppServiceApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace AppServiceApi.Controllers
{
    public class BaseController : ApiController
    {
        protected const string ERR_INVALID_INPUT = "ERROR: Invalid input / required data missing";
        protected const string ERR_INVALID_AUTH = "User not authenticated";
        protected const string ERR_EMPTY_TOKEN = "Token payload is missing ";
        protected const string ERR_MISSING_PERMISSION = "User not authorized for current operation";
        protected string Email = string.Empty;
        protected string CustomerName = string.Empty;
        protected string UserId = string.Empty;
        protected string CustomerId = string.Empty;
        protected List<string> PermiCheckList = new List<string>();

        protected BaseAuth baseAuth = new BaseAuth();
        protected string token = "";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await Task.Run(() => { return "ApiService " + ConfigurationManager.AppSettings["ApiVersion"]; }));
        }

        protected bool IsAuthorised(out string errorMessage)
        {
            token = Request.Headers.Authorization.ToString();

            var principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            if (principal == null)
            {
                errorMessage = ERR_INVALID_AUTH;
                return false;
            }
            var permList = principal.Claims.Where(c => c.Type == "appData");

            if (permList == null || permList.Count<Claim>() <= 0)
            {
                errorMessage = ERR_EMPTY_TOKEN;
                return false;
            }

            Email = principal.Claims.First(c => c.Type.Contains("emailaddress")).Value;
            if (principal.Claims.Any(c => c.Type == ("customerName")))
                CustomerName = principal.Claims.First(c => c.Type.Contains("customerName")).Value;
            if (principal.Claims.Any(c => c.Type == ("userId")))
                UserId = principal.Claims.First(c => c.Type.Contains("userId")).Value;
            if (principal.Claims.Any(c => c.Type == ("customerId")))
                CustomerId = principal.Claims.First(c => c.Type.Contains("customerId")).Value;
                     

            foreach (var l2 in permList)
            {
                dynamic auth = JsonConvert.DeserializeObject(l2.Value);
                var appServiceAuth = ((JObject)auth)["appService"];     //TODO improve this by using classes , gave error while using class hence this alternate method , recheck.

                if (appServiceAuth == null)
                    appServiceAuth = ((JObject)auth)[""]["appService"];

                if (appServiceAuth != null)
                {
                    bool result = appServiceAuth["modebase"].ToObject<bool>();
                    if (result == false)
                    {
                        errorMessage = ERR_MISSING_PERMISSION;
                        return false;
                    }
                    errorMessage = string.Empty;
                    return true;
                }
            }
            errorMessage = ERR_MISSING_PERMISSION;
            return false;
        }

        protected IHttpActionResult ErrorAsync(Exception ex, string uri, object value = null)
        {
            var message = ex.Message;
            var messagestr = "";
            messagestr += !string.IsNullOrEmpty(UserId) ? Environment.NewLine + "USER ID          : " + UserId : "";
            messagestr += !string.IsNullOrEmpty(Email) ? Environment.NewLine + "EMAIL ID         : " + Email : "";
            messagestr += !string.IsNullOrEmpty(CustomerId) ? Environment.NewLine + "CUSTOMER ID      : " + CustomerId : "";
            messagestr += !string.IsNullOrEmpty(CustomerName) ? Environment.NewLine + "CUSTOMER NAME    : " + CustomerName : "";
            messagestr += Environment.NewLine + "REQUEST URI : " + uri;

            if (value != null)
            {
                var input_value = Newtonsoft.Json.JsonConvert.SerializeObject(value, Formatting.Indented);
                if (!string.IsNullOrEmpty(input_value.ToString()))
                    messagestr += Environment.NewLine + "INPUT PARAMETER: " + Environment.NewLine + input_value.ToString();
            }

            messagestr += Environment.NewLine + "ERROR INFO : " + Environment.NewLine + message;
            if (ex.InnerException != null) messagestr += ex.InnerException;
            if (ex.StackTrace != null) messagestr += ex.StackTrace;

            Trace.TraceError(messagestr);
            return InternalServerError(ex);
        }
    }
   
}
