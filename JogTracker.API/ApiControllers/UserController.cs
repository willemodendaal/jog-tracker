using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using JogTracker.Api.Models.JsonResults;
using JogTracker.Api.Utils;
using JogTracker.DomainModel;
using JogTracker.Services;

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
            PagedModel<JogTrackerUser> users = _userService.GetUsers(model.PageIndex.Value, model.PageSize.Value);
            List<UserJsonResult> jsonUsers = new Mapper<JogTrackerUser, UserJsonResult>().Map(users.Items).ToList();

            return Ok(new PagingResults(model.PageIndex.Value, model.PageSize.Value, users.TotalResults, jsonUsers));
        }
    }
}