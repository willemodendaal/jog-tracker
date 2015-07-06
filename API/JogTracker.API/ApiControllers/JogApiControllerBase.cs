using System.Web.Http;
using JogTracker.Common;
using Microsoft.AspNet.Identity;

namespace JogTracker.Api.ApiControllers
{
    public class JogApiControllerBase : ApiController
    {
        protected string GetCurrentUserId()
        {
            return RequestContext.Principal.Identity.GetUserId();
        }

        protected string GetCurrentUserEmail()
        {
            return RequestContext.Principal.Identity.GetUserName(); //Username is email.
        }

        protected bool UserIsAdmin()
        {
            return RequestContext.Principal.IsInRole(GlobalConfig.AdminRole);
        }

        protected bool UserIsUserManager()
        {
            return RequestContext.Principal.IsInRole(GlobalConfig.UserManager);
        }
    }
}