using System;
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
        [HttpGet]
        [Validate]
        public async Task<IHttpActionResult> GetJogs([FromUri] JogFilterBindingModel filter)
        {
            PagedModel<JogEntry> allJogs =
                (await
                    _repo.FindAsync(
                        filter.PageIndex.Value,
                        filter.PageSize.Value,
                        filter.FromDate.Value,
                        filter.ToDate.Value,
                        GetCurrentUserId(),
                        UserIsAdmin()));

            ICollection<JogJsonResult> jsonResult = new Mapper<JogEntry, JogJsonResult>().Map(allJogs.Items);
            return Ok(new PagingResults(filter.PageIndex.Value, filter.PageSize.Value, allJogs.TotalResults, jsonResult));
        }

        [Route("{jogId}")]
        [HttpGet]
        [Validate]
        public async Task<IHttpActionResult> GetJog(string jogId)
        {
            JogEntry jog = await _repo.GetAsync(jogId, GetCurrentUserId(), UserIsAdmin());

            if (jog == null)
            {
                return NotFound();
            }

            JogJsonResult jsonResult = new Mapper<JogEntry, JogJsonResult>().Map(jog);
            return Ok(jsonResult);
        }

        [Route("{jogId}")]
        [HttpDelete]
        [Validate]
        public async Task<IHttpActionResult> DeleteJog(string jogId)
        {
            try
            {
                await _repo.DeleteAsync(jogId, GetCurrentUserId(), UserIsAdmin());
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                //When people try and get access to jogs they shouldn't have access to.
                return BadRequest("Invalid jogId.");
            }
        }

        [Route("new")]
        [HttpPost]
        [Validate]
        public async Task<IHttpActionResult> CreateNew(JogBindingModel jog)
        {
            JogEntry newJogEntry =
                await _repo.CreateNewAsync(jog.DateTime, jog.DistanceKM, jog.Duration, GetCurrentUserId());

            JogJsonResult jsonJog = new Mapper<JogEntry, JogJsonResult>().Map(newJogEntry);

            return Ok(jsonJog);
        }

        [Route("{jogId}/update")]
        [HttpPut]
        [Validate]
        public async Task<IHttpActionResult> Update(string jogId, JogBindingModel jog)
        {
            try
            {
                JogEntry updatedEntry =
                    await _repo.UpdateAsync(
                        jogId,
                        jog.DateTime,
                        jog.DistanceKM,
                        jog.Duration,
                        GetCurrentUserId(),
                        UserIsAdmin());

                JogJsonResult jsonJog = new Mapper<JogEntry, JogJsonResult>().Map(updatedEntry);

                return Ok(jsonJog);
            }
            catch (UnauthorizedAccessException)
            {
                //When people try and get access to jogs they shouldn't have access to.
                return BadRequest("Invalid jogId.");
            }
        }
    }
}