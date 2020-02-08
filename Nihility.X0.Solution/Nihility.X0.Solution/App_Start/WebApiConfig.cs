using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Nihility.X0.Solution
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Cấu hình Web API và các dịch vụ khác          
            config.SuppressDefaultHostAuthentication();

            // Cấu hình cho Web API chỉ dùng Bearer Token cho quá trình chứng thực
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

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
