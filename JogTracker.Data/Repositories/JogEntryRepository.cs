using JogTracker.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogTracker.Data.Repositories
{
    public interface IJogEntryRepository
    {
        IEnumerable<JogEntry> All();
    }

    public class JogEntryRepository : IJogEntryRepository
    {
        JogDbContext _dbContext;

        public JogEntryRepository(JogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<JogEntry> All()
        {
            //Return all jog entries from the database. No filtering.
            return _dbContext.JogEntries.ToList();
        }
    }
}
