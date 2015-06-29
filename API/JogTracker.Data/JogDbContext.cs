using JogTracker.DomainModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Data
{
    /// <summary>
    /// Inherit from IdentityDbContext (.AspNet.Identity.EntityFramework) to indicate we
    /// want to make use of ASP.NET Identity security.
    /// </summary>
    public class JogDbContext : IdentityDbContext<JogTrackerUser>
    {
        
        public JogDbContext() : base("JobDbContextConnection")
        {
        }

        public DbSet<JogEntry> JogEntries { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //So that table names are singular, not plural. More predictable.
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
