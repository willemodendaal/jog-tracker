using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace JogTracker.Api.ApiControllers
{
    public class JogApiControllerBase : ApiController
    {
        protected string GetCurrentUserId()
        {
            return RequestContext.Principal.Identity.GetUserId();
        }
    }
}