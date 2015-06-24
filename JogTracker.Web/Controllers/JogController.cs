using JogTracker.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JogTracker.Web.Controllers
{
    /// <summary>
    /// Represents the "Jog" resource. A job stores details about a person's jogging session.
    /// </summary>
    public class JogController : ApiController
    {
        public IHttpActionResult GetJogs(JogFilterBindingModel filter)
        {
            return Ok(GetMockJogs());
        }

        private List<JogBindingModel> GetMockJogs()
        {
            return new List<JogBindingModel>()
            {
                new JogBindingModel() { DateTime = new DateTime(1992, 05, 20, 15,0,0), DistanceKM = 5.4F },
                new JogBindingModel() { DateTime = new DateTime(1992, 05, 21, 15,0,0), DistanceKM = 5.3F }

            }; 
        }
    }
}
