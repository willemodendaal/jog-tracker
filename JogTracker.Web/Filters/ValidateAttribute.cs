using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

namespace JogTracker.Api.Filters
{
    /// <summary>
    /// Apply attribute to actions where we need to validate the modelState and return 
    /// a simple BadRequest response if it is invalid.
    /// </summary>
    public class ValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            //Set error response on the context, if the ModelState is invalid.
            if (!context.ModelState.IsValid)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.ModelState);
            }
        }
    }
}