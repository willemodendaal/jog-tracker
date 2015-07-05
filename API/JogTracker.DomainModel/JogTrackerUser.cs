using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JogTracker.Common;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JogTracker.DomainModel
{
    public class JogTrackerUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
