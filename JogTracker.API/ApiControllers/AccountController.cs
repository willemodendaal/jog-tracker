using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using JogTracker.Services;
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
        private IRegistrationService _registrationService;

        public AccountController(IRegistrationService regService)
        {
            _registrationService = regService;
        }

        [Route("register")]
        [HttpPost]
        [Validate]
        public IHttpActionResult Register(RegisterBindingModel model)
        {
            _registrationService.Register(model.UserName, model.Email, model.Password);
            return Ok();
        }


    }
}