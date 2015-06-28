using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using JogTracker.DomainModel;

namespace JogTracker.Data.Repositories
{
    public interface IJogEntryRepository
    {
        Task<ICollection<JogEntry>> AllAsync(string userId);
        Task<JogEntry> CreateNewAsync(DateTime dateTime, float distanceKm, TimeSpan duration, string userId);
    }

    public class JogEntryRepository : IJogEntryRepository
    {
        private JogDbContext _dbContext;

        public JogEntryRepository(JogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<JogEntry>> AllAsync(string userId)
        {
            //Return all jog entries from the database. No filtering.
            return await (_dbContext.JogEntries).ToListAsync();
        }

        public async Task<JogEntry> CreateNewAsync(DateTime dateTime, float distanceKm, TimeSpan duration, string userId)
        {
            var newJog = new JogEntry()
            {
                DistanceKM = distanceKm,
                DateTime = dateTime,
                Duration = duration,
                UserId = userId
            };

            _dbContext.JogEntries.Add(newJog);
            await _dbContext.SaveChangesAsync();
            return newJog;
        }

    }
}