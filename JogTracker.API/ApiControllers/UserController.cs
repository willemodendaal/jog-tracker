using System.Collections;
using System.Web.Http;
using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using JogTracker.DomainModel;
using JogTracker.Services;
using System.Collections.Generic;
using System.Linq;
using JogTracker.Api.Models.JsonResults;
using JogTracker.Api.Utils;

namespace JogTracker.Api.ApiControllers
{
    [Authorize(Roles = "administrator")]
    [RoutePrefix("api/v1/user")]
    public class UserController : ApiController
    {
        private readonly IUserAdminService _userService;

        public UserController(IUserAdminService userService)
        {
            _userService = userService;
        }

        [Route("")]
        [Validate]
        [HttpGet]
        public IHttpActionResult Users([FromUri] UserFilterBindingModel model)
        {
            List<JogEntryUser> users = _userService.GetUsers(model.PageIndex.Value, model.PageSize.Value);
            List<UserJsonResult> jsonUsers = new Mapper<JogEntryUser, UserJsonResult>().Map(users).ToList();

            return Ok(jsonUsers);
        }


    }
}