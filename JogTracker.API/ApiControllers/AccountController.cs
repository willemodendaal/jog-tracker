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
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            string errorResult = await _userAdminService.RegisterAsync(model.Email, model.Password);

            if (errorResult != null)
            {
                //Return info about why request failed. (i.e. password not complex enough)
                //  Should be picked up by validation layer in most cases.
                return BadRequest(errorResult);
            }

            return Ok();
        }

        [Route("requestResetPwd")]
        [HttpPost]
        [Validate]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RequestResetPassword(RequestResetPasswordBindingModel model)
        {
            //Email is sent by the userAdminService. Nothing returned from here.
            await _userAdminService.RequestResetPasswordAsync(model.Email);
            return Ok();
        }


        [Route("resetPwd")]
        [HttpPost]
        [Validate]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindingModel model)
        {
            string errorResult = await _userAdminService.ResetPasswordAsync(model.UserId, model.Token, model.NewPassword);

            if (errorResult != null)
            {
                //Return info about why request failed. (i.e. password not complex enough)
                //  Should be picked up by validation layer in most cases.
                return BadRequest(errorResult);
            }

            return Ok();
        }

    }
}