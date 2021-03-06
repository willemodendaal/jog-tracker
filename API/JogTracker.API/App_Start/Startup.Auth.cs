﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using JogTracker.Api.Providers;
using JogTracker.Data;
using Microsoft.Owin.Security.DataProtection;
using JogTracker.Common;
using JogTracker.DomainModel;

namespace JogTracker.Api
{
    public partial class Startup
    {

        static Startup()
        {
            PublicClientId = "jogTracker.web";

            UserManagerFactory = () => new UserManager<JogTrackerUser>(new UserStore<JogTrackerUser>(new JogDbContext()));

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new JogTrackerOAuthProvider(PublicClientId, UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = false
            };


        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static Func<UserManager<JogTrackerUser>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            System.Diagnostics.Trace.TraceInformation("Configuring webApi authentication.");
            EnableCors(app);

            // This is what allows our front-end to submit tokens instead of userName/password with every request.
            app.UseOAuthBearerTokens(OAuthOptions);
            GlobalSharedSecurity.DataProtectionProvider = app.GetDataProtectionProvider();
        }

        private void EnableCors(IAppBuilder app)
        {
            System.Diagnostics.Trace.TraceInformation("Configuring CORS.");

            app.Use(async (context, next) =>
            {
                IOwinRequest req = context.Request;
                IOwinResponse res = context.Response;

                //Allow requests for authentication to /Token (because we cannot use normal Cors filters here).
                System.Diagnostics.Trace.TraceInformation(string.Concat("Got Request: ", req.Path));

                if (req.Path.StartsWithSegments(new PathString("/Token")))
                {
                    var origin = req.Headers.Get("Origin");
                    System.Diagnostics.Trace.TraceInformation(string.Concat("Got /token request for origin: ", origin, ". Checking against allowed: ", string.Join("  ", GlobalConfig.AllowedCorsOrigins)));
                    if (!string.IsNullOrEmpty(origin) && GlobalConfig.AllowedCorsOrigins.Any(o => o.Equals(origin, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        res.Headers.Set("Access-Control-Allow-Origin", origin);
                    }

                    //Special logic for OPTIONS request. Needed if doing anything other than basic GETs or POSTs. 
                    if (req.Method == "OPTIONS")
                    {
                        res.StatusCode = 200;
                        res.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Methods", "GET", "POST");
                        res.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Headers", "authorization", "content-type");
                        return;
                    }
                }
                await next();
            });
        }
    }
}
