using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models.BadgeApplicationDetailsWindow
{
    public class BadgeApplicationStats
    {
        public class YearStats
        {
            public int TourCount { get; set; }
            public int PointSum { get; set; }
            public int Year { get; set; }
            public int CustomRouteCount { get; set; }
            public double AveragePointsPerTour { get { return PointSum / (double) TourCount; } }
        }

        public int PointsSum { get; set; }
        public int TourCount { get; set; }
        public int CustomRouteCount { get; set; }
        public double AveragePointsInYear { get; set; }

        public IEnumerable<YearStats> YearStatistics { get; set; }
    }
}
