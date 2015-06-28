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
using JogTracker.Api.Models.JsonResults;
using JogTracker.Api.Utils;
using JogTracker.Common;
using JogTracker.DomainModel;
using JogTracker.Services.Responses;

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
            RegistrationResult result = await _userAdminService.RegisterAsync(model.Email, model.Password, model.FirstName, model.LastName);

            if (!result.Succeeded)
            {
                //Return info about why request failed. (i.e. password not complex enough)
                //  Should be picked up by validation layer in most cases.
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [Route("registerAsAdmin")]
        [Authorize(Roles = "administrator")] //Only admin role can do this. Will skip email validation (once that is added).
        [HttpPost]
        [Validate]
        public async Task<IHttpActionResult> RegisterAsAdmin(RegisterBindingModel model)
        {
            RegistrationResult result = await _userAdminService.RegisterAsync(model.Email, model.Password, model.FirstName, model.LastName);

            if (! result.Succeeded)
            {
                //Return info about why request failed. (i.e. password not complex enough)
                //  Should be picked up by validation layer in most cases.
                return BadRequest(result.ErrorMessage);
            }

            //This action returns the user as well, so that the administrator has the new user Id.
            var jsonUser = new Mapper<JogTrackerUser, UserJsonResult>().Map(result.User);
            return Ok(jsonUser);
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