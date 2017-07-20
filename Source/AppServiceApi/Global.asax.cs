using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using AppServiceApi.Util.Misc;

namespace AppServiceApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            Trace.TraceInformation("IAZI Application Service started.");

        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Trace.TraceError(ExceptionHelper.GetExceptionMessage(exception, true, true));
        }

    }
}
