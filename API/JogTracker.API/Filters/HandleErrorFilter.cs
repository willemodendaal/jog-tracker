using JogTracker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace JogTracker.Api.Filters
{
    public class HandleErrorFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            System.Diagnostics.Trace.TraceError("Error. Message: {0}, Stack: {1}",
                context.Exception.Message,
                context.Exception.StackTrace);

            if (context.Response == null)
                return;

            if (context.Response.StatusCode == HttpStatusCode.InternalServerError)
            {
                context.Response.Content = new StringContent(GlobalConfig.FriendlyGenericError, Encoding.UTF8, "application/json");
            }
        }
    }
}