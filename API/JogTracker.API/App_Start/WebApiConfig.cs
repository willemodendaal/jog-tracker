using JogTracker.Api.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using JogTracker.Common;

namespace JogTracker.Api
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            config.EnableSystemDiagnosticsTracing();

            // Enable CORS on all our controllers.
            var corsConfig = new EnableCorsAttribute(string.Join(",", GlobalConfig.AllowedCorsOrigins), "*", "*");
            config.EnableCors(corsConfig);

            config.Filters.Add(new HandleErrorFilter());

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
