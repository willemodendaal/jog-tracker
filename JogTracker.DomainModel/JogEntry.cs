using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JogTracker.DomainModel
{
    /// <summary>
    /// Represents a single jog entry (date, time, distance, average speed).
    /// </summary>
    public class JogEntry
    {
        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(0.0F, 300F)]
        public float DistanceKM { get; set; }

        public float AverageSpeedKMH
        {
            get
            {
                return DistanceKM / Duration.Hours;
            }
        }
    }
}
