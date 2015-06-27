using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using JogTracker.Api.Providers;
using JogTracker.Data;

namespace JogTracker.Api
{
    public partial class Startup
    {

        static Startup()
        {
            PublicClientId = "jogTracker.web";

            UserManagerFactory = () => new UserManager<IdentityUser>(new UserStore<IdentityUser>(new JogDbContext()));

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

        public static Func<UserManager<IdentityUser>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            // This is what allows our front-end to submit tokens instead of userName/password with every request.
            app.UseOAuthBearerTokens(OAuthOptions);

            EnableCors(app);
        }

        private void EnableCors(IAppBuilder app)
        {

            app.Use(async (context, next) =>
            {
                IOwinRequest req = context.Request;
                IOwinResponse res = context.Response;

                //Allow requests for authentication to /Token (because we cannot use normal Cors filters here).
                if (req.Path.StartsWithSegments(new PathString("/Token")))
                {
                    var origin = req.Headers.Get("Origin");
                    if (!string.IsNullOrEmpty(origin) && WebApiConfig.AllowedCorsOrigins.Any(o => o.Equals(origin, StringComparison.InvariantCultureIgnoreCase)) )
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
