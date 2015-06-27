using JogTracker.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JogTracker.Api.ApiControllers
{
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
        [Route("register")]
        [HttpPost]
        public IHttpActionResult Register(RegisterBindingModel model)
        {
            return Ok();
        }


    }
}