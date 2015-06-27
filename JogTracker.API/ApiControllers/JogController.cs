using JogTracker.Data.Repositories;
using JogTracker.DomainModel;
using JogTracker.Api.Filters;
using JogTracker.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JogTracker.Api.ApiControllers
{
    /// <summary>
    /// Represents the "Jog" resource. A job stores details about a person's jogging session.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/jog")]
    public class JogController : ApiController
    {
        IJogEntryRepository _repo;

        public JogController(IJogEntryRepository repo)
        {
            _repo = repo;
        }

        [Route("")]
        [Validate]
        public IHttpActionResult GetJogs([FromUri]JogFilterBindingModel filter)
        {
            IEnumerable<JogEntry> allJogs = _repo.All();
            return Ok(allJogs);
        }

    }
}
