using JogTracker.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Data.Init
{
    /// <summary>
    /// Used to seed the database while developing. Not for production use.
    /// </summary>
    public class JogTrackerDbInitializer : DropCreateDatabaseIfModelChanges<JogDbContext>
    {
        protected override void Seed(JogDbContext context)
        {
            context.JogEntries.AddRange(new[] {
                new JogEntry() { DistanceKM = 3, StartDateTime = DateTime.Now.AddMonths(-2), Duration = new TimeSpan(0,30,2) },
                new JogEntry() { DistanceKM = 3.5, StartDateTime = DateTime.Now.AddMonths(-1),  Duration = new TimeSpan(0,26,18)  },
            });

            context.SaveChanges();
        }
    }
}
