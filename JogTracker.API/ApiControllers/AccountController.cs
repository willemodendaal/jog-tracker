using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using JogTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JogTracker.Api.ApiControllers
{
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
        private IUserAdminService _userAdminService;

        public AccountController(IUserAdminService regService)
        {
            _userAdminService = regService;
        }

        [Route("register")]
        [HttpPost]
        [Validate]
        public IHttpActionResult Register(RegisterBindingModel model)
        {
            _userAdminService.Register(model.Email, model.Password);
            return Ok();
        }

        [Route("requestResetPwd")]
        [HttpPost]
        [Validate]
        public async Task<IHttpActionResult> RequestResetPassword(RequestResetPasswordBindingModel model)
        {
            //Email is sent by the userAdminService. Nothing returned from here.
            await _userAdminService.RequestResetPassword(model.UserName);
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