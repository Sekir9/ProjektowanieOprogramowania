using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models.BadgeApplicationDetailsWindow
{
    public class BadgeApplicationDetailsWindowPayload
    {
        public class TourEntry
        {
            public DateTime TourStart { get; set; }
            public DateTime TourEnd { get; set; }
            public int TourLength { get { return (TourEnd - TourStart).Days + 1; } }
            public string MountainGroupsString { get; set; }
            public int RouteCount { get; set; }
            public int Points { get; set; }
        }

        public string TuristName { get; set; }
        public string ApplicationFor { get; set; }
        public string TuristHasBadges { get; set; }

        public IEnumerable<TourEntry> Tours { get; set; }
        public string Description { get; set; }

        public BadgeApplicationStats Stats { get; set; }
    }
}
