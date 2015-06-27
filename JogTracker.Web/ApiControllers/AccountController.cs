using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JogTracker.Api.ApiControllers
{
    [RoutePrefix("api/v1")]
    public class AccountController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public void Login()
        {
        }

        [Route("logout")]
        [HttpPost]
        public void Logout()
        {
        }

    }
}