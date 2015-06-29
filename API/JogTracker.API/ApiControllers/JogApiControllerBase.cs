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

        protected bool UserIsAdmin()
        {
            return RequestContext.Principal.IsInRole(GlobalConfig.AdminRole);
        }
    }
}