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
        private IUserAdminService _registrationService;

        public AccountController(IUserAdminService regService)
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

        [Route("requestResetPwd")]
        [HttpPost]
        [Validate]
        public IHttpActionResult RequestResetPassword()
        {
            return Ok();
        }


        [Route("resetPwd")]
        [HttpPost]
        [Validate]
        public IHttpActionResult ResetPassword()
        {
            return Ok();
        }

    }
}