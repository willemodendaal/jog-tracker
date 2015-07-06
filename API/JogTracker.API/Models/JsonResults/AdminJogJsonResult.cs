using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JogTracker.Api.Utils;
using JogTracker.DomainModel;

namespace JogTracker.Api.Models.JsonResults
{
    public class AdminJogJsonResult : JsonResultBase, IMappable<JogEntry, AdminJogJsonResult>
    {
        public string id { get; set; }
        public string date { get; set; }
        public TimeSpan duration { get; set; }
        public float distanceKm { get; set; }
        public float averageKmh { get; set; }
        public string email { get; set; } //User email

        public AdminJogJsonResult Map(JogEntry source)
        {
            return new AdminJogJsonResult ()
            {
                id = source.ID.ToString(),
                date = SafeJsonDate(source.DateTime),
                distanceKm = source.DistanceKM,
                duration = source.Duration,
                averageKmh = source.AverageSpeedKMH,
                email = source.UserEmail 
            };
        }
    }
}