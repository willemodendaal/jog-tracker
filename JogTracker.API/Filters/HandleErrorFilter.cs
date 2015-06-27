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
            if (context.Response.StatusCode == HttpStatusCode.InternalServerError)
            {
                //Log and return friendly generic message.
                System.Diagnostics.Trace.TraceError("Error. Message: {0}, Stack: {1}",
                    context.Exception.Message,
                    context.Exception.StackTrace);

                context.Response.Content = new StringContent(Config.FriendlyGenericError, Encoding.UTF8, "application/json");

            }
        }
    }
}