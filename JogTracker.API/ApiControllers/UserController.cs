using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using JogTracker.Api.Models.JsonResults;
using JogTracker.Api.Utils;
using JogTracker.DomainModel;
using JogTracker.Services;
using JogTracker.Services.Responses;

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
        public async Task<IHttpActionResult> Users([FromUri] UserFilterBindingModel model)
        {
            PagedModel<JogTrackerUser> users = await _userService.GetUsersAsync(model.PageIndex.Value, model.PageSize.Value);
            List<UserJsonResult> jsonUsers = new Mapper<JogTrackerUser, UserJsonResult>().Map(users.Items).ToList();

            return Ok(new PagingResults(model.PageIndex.Value, model.PageSize.Value, users.TotalResults, jsonUsers));
        }

        [Route("{userId}")]
        [Validate]
        [HttpGet]
        public async Task<IHttpActionResult> User(string userId)
        {
            JogTrackerUser user = await _userService.GetUserAsync(userId);
            UserJsonResult jsonUser = new Mapper<JogTrackerUser, UserJsonResult>().Map(user);

            return Ok(jsonUser);
        }


        [Route("{userId}/update")]
        [HttpPut]
        [Validate]
        public async Task<IHttpActionResult> Update(string userId, UserUpdateBindingModel model)
        {
            UpdateResult result = await _userService.UpdateAsync(userId, model.FirstName, model.LastName, model.Email);

            if (!result.Succeeded)
            {
                //Return info about why request failed. (i.e. password not complex enough)
                //  Should be picked up by validation layer in most cases.
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }
    }
}