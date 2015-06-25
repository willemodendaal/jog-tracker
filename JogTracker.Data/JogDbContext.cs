using JogTracker.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Data
{
    public class JogDbContext : DbContext
    {
        public JogDbContext() : base("JobDbContextConnection")
        {
        }

        public DbSet<JogEntry> JogEntries { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //So that table names are singular, not plural. More predictable.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
