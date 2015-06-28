using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using JogTracker.DomainModel;

namespace JogTracker.Data.Repositories
{
    public interface IJogEntryRepository
    {
        Task<PagedModel<JogEntry>> AllAsync(int pageIndex, int pageSize, string userId);
        Task<JogEntry> CreateNewAsync(DateTime dateTime, float distanceKm, TimeSpan duration, string userId);
    }

    public class JogEntryRepository : IJogEntryRepository
    {
        private JogDbContext _dbContext;

        public JogEntryRepository(JogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedModel<JogEntry>> AllAsync(int pageIndex, int pageSize, string userId)
        {
            int totalResults = await _dbContext.JogEntries
                .Where(j => j.UserId == userId)
                .CountAsync();

            var result = await (_dbContext.JogEntries
                .OrderByDescending(j => j.DateTime)
                .Where(j => j.UserId == userId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
            ).ToListAsync();

            return new PagedModel<JogEntry>(pageIndex, pageSize, totalResults, result);
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