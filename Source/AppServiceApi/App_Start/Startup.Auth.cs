using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AppServiceApi
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var issuerV1 = ConfigurationManager.AppSettings["TokenIssuerV1"];
            var issuerV2 = ConfigurationManager.AppSettings["TokenIssuerV2"];

            var audience_clientdId = "iJbqXTj9oETMZZWW7q6cRCygNQ4VC0oU";
            var key_clientdSecret = Convert.FromBase64String("Fw5E0kHY6O2XpQD4dkbXRuWctHNYYTkmwUdIDxSI9soD4X9yZOzm6Hgt8iXCRrEp");
            var x = new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audience_clientdId },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(issuerV1, key_clientdSecret),   // V1 token for backward compatibility
                    new SymmetricKeyIssuerSecurityTokenProvider(issuerV2, key_clientdSecret)    // V2 token
                }
            };
            app.UseJwtBearerAuthentication(x);
        }
    }
}