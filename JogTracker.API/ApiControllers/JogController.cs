using JogTracker.Data.Repositories;
using JogTracker.DomainModel;
using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using JogTracker.Api.Models.JsonResults;
using JogTracker.Api.Utils;
using Microsoft.AspNet.Identity;

namespace JogTracker.Api.ApiControllers
{
    /// <summary>
    /// Represents the "Jog" resource. A job stores details about a person's jogging session.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/jog")]
    public class JogController : JogApiControllerBase
    {
        IJogEntryRepository _repo;

        public JogController(IJogEntryRepository repo)
        {
            _repo = repo;
        }

        [Route("")]
        [Validate]
        public async Task<IHttpActionResult> GetJogs([FromUri]JogFilterBindingModel filter)
        {
            ICollection<JogEntry> allJogs = (await _repo.AllAsync(base.GetCurrentUserId())).ToList();
            ICollection<JogJsonResult> jsonResult = new Mapper<JogEntry, JogJsonResult>().Map(allJogs);
            return Ok(jsonResult);
        }

        [Route("new")]
        [Validate]
        [HttpPost]
        public async Task<IHttpActionResult> CreateNew(JogBindingModel jog)
        {
            JogEntry newJogEntry = await _repo.CreateNewAsync(jog.DateTime, jog.DistanceKM, jog.Duration, base.GetCurrentUserId());
            JogJsonResult jsonJog = new Mapper<JogEntry, JogJsonResult>().Map(newJogEntry);
            
            return Ok(jsonJog);
        }
        

    }
}
