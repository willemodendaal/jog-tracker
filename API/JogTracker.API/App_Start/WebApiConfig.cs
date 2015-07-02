using JogTracker.Api.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JogTracker.Api
{
    public static class WebApiConfig
    {
        internal static readonly string[] AllowedCorsOrigins = new[] { "https://localhost:44302", "http://localhost:3000" }; //TODO: Make configurable

        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new HandleErrorFilter());
            config.EnableSystemDiagnosticsTracing();

            // Enable CORS on all our controllers.
            var corsConfig = new EnableCorsAttribute(string.Join(",", AllowedCorsOrigins), "*", "*");
            config.EnableCors(corsConfig);

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
