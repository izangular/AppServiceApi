

using AppServiceApi.Util.Log;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Tracing;

namespace AppServiceApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableSystemDiagnosticsTracing();

            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());
            //By default return the JSON (when called from chrome browser)
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //Enabling Cross-Origin Requests
            //Cross Origin Resource Sharing (CORS) is a W3C standard that allows a server to relax the same-origin policy.
            //Using CORS, a server can explicitly allow some cross-origin requests while rejecting others.
            //CORS is safer and more flexible than earlier techniques such as JSONP.
            config.EnableCors(new EnableCorsAttribute("*", "*", "GET,POST,OPTIONS"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
