﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JogTracker.DomainModel;

namespace JogTracker.Data.Repositories
{
    public interface IJogEntryRepository
    {
        Task<PagedModel<JogEntry>> FindAsync(
            int pageIndex, 
            int pageSize, 
            DateTime startDateTime, 
            DateTime endDateTime,
            string userId, 
            bool isAdmin);

        Task<ICollection<JogEntry>> FindForWeekAsync(
            DateTime week,
            string userId,
            bool isAdmin);

        Task<JogEntry> CreateNewAsync(DateTime dateTime, float distanceKm, TimeSpan duration, string userId, string userEmail);
        Task<JogEntry> GetAsync(string jogId, string userId, bool isAdmin);

        Task<JogEntry> UpdateAsync(
            string jogId, 
            DateTime dateTime, 
            float distanceKm, 
            TimeSpan duration,
            string getCurrentUserId, 
            bool isAdmin);

        Task DeleteAsync(string jogId, string userId, bool isAdmin);
        Task<PagedModel<JogEntry>> GetAllAsync(string userId, bool userIsAdmin);
    }

    public class JogEntryRepository : IJogEntryRepository
    {
        private JogDbContext _dbContext;

        public JogEntryRepository(JogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedModel<JogEntry>> GetAllAsync(string userId, bool userIsAdmin)
        {
            var result = await (_dbContext.JogEntries
                .OrderByDescending(j => j.DateTime)
                ).ToListAsync();

            return new PagedModel<JogEntry>(0, result.Count, result.Count, result);
        }

        public async Task<ICollection<JogEntry>> FindForWeekAsync(
           DateTime week,
           string userId,
           bool isAdmin)
        {
            DateTime weekStart = GetStartOfWeek(week);
            DateTime weekEnd = weekStart.AddDays(7); //To previous night.

            var result = await (_dbContext.JogEntries
                .OrderByDescending(j => j.DateTime)
                .Where(MatchJogEntries(weekStart, weekEnd, userId, isAdmin))
                ).ToListAsync();

            return result;
        }

        public async Task<PagedModel<JogEntry>> FindAsync(
            int pageIndex, 
            int pageSize, 
            DateTime startDateTime,
            DateTime endDateTime, 
            string userId, 
            bool isAdmin)
        {
            int totalResults = await _dbContext.JogEntries
                .Where(MatchJogEntries(startDateTime, endDateTime, userId, isAdmin))
                .CountAsync();

            var result = await (_dbContext.JogEntries
                .OrderByDescending(j => j.DateTime)
                .Where(MatchJogEntries(startDateTime, endDateTime, userId, isAdmin))
                .Skip(pageIndex*pageSize)
                .Take(pageSize)
                ).ToListAsync();

            return new PagedModel<JogEntry>(pageIndex, pageSize, totalResults, result);
        }

        public async Task<JogEntry> GetAsync(string jogId, string userId, bool isAdmin)
        {
            var jogEntry =
                await _dbContext.JogEntries.FirstOrDefaultAsync(j => j.ID.ToString() == jogId && (j.UserId == userId || isAdmin));

            return jogEntry;
        }

        public async Task DeleteAsync(string jogId, string userId, bool isAdmin)
        {
            var jog = await GetAsync(jogId, userId, isAdmin);
            if (jog == null)
            {
                throw new UnauthorizedAccessException(
                    string.Format("Attempt to update jog {0} which does not exist for user {1}.", jogId, userId));
            }

            _dbContext.JogEntries.Remove(jog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<JogEntry> CreateNewAsync(DateTime dateTime, float distanceKm, TimeSpan duration, string userId, string userEmail)
        {
            var newJog = new JogEntry()
            {
                DistanceKM = distanceKm,
                DateTime = dateTime,
                Duration = duration,
                UserId = userId,
                UserEmail = userEmail
            };

            _dbContext.JogEntries.Add(newJog);
            await _dbContext.SaveChangesAsync();
            return newJog;
        }

        public async Task<JogEntry> UpdateAsync(string jogId, DateTime dateTime, float distanceKm, TimeSpan duration,
            string userId, bool isAdmin)
        {
            var jog = await GetAsync(jogId, userId, isAdmin);
            if (jog == null)
            {
                throw new UnauthorizedAccessException(
                    string.Format("Attempt to update jog {0} which does not exist for user {1}.", jogId, userId));
            }

            jog.DateTime = dateTime;
            jog.DistanceKM = distanceKm;
            jog.Duration = duration;
            await _dbContext.SaveChangesAsync();

            return jog;
        }

        /// <summary>
        /// Returns true if JogEntry belongs to the user and is within the start/end date range.
        /// </summary>
        private static Expression<Func<JogEntry, bool>> MatchJogEntries(
            DateTime startDateTime, 
            DateTime endDateTime,
            string userId, 
            bool isAdmin)
        {
            return j => (j.UserId == userId || isAdmin) && j.DateTime >= startDateTime && j.DateTime < endDateTime;
        }

        private DateTime GetStartOfWeek(DateTime dt)
        {
            int diff = dt.DayOfWeek - DayOfWeek.Sunday;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}