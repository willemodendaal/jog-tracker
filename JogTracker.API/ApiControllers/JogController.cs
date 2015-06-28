using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using JogTracker.Api.Models.JsonResults;
using JogTracker.Api.Utils;
using JogTracker.Data.Repositories;
using JogTracker.DomainModel;

namespace JogTracker.Api.ApiControllers
{
    /// <summary>
    /// Represents the "Jog" resource. A job stores details about a person's jogging session.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/jog")]
    public class JogController : JogApiControllerBase
    {
        private IJogEntryRepository _repo;

        public JogController(IJogEntryRepository repo)
        {
            _repo = repo;
        }

        [Route("")]
        [Validate]
        public async Task<IHttpActionResult> GetJogs([FromUri] JogFilterBindingModel filter)
        {
            PagedModel<JogEntry> allJogs =
                (await _repo.FindAsync(filter.PageIndex.Value, filter.PageSize.Value, filter.FromDate.Value, filter.ToDate.Value, GetCurrentUserId()));
            ICollection<JogJsonResult> jsonResult = new Mapper<JogEntry, JogJsonResult>().Map(allJogs.Items);
            return Ok(new PagingResults(filter.PageIndex.Value, filter.PageSize.Value, allJogs.TotalResults, jsonResult));
        }

        [Route("new")]
        [Validate]
        [HttpPost]
        public async Task<IHttpActionResult> CreateNew(JogBindingModel jog)
        {
            JogEntry newJogEntry =
                await _repo.CreateNewAsync(jog.DateTime, jog.DistanceKM, jog.Duration, GetCurrentUserId());
            JogJsonResult jsonJog = new Mapper<JogEntry, JogJsonResult>().Map(newJogEntry);

            return Ok(jsonJog);
        }
    }
}