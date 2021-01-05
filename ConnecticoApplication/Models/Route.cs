using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Length { get; set; }
        public int SumOfClimbs { get; set; }
        public int Points { get { return GetPoints(); } }

        public bool isStandardRoute { get { return StandardRouteData != null; } }
        public StandardRouteData StandardRouteData { get; set; }

        public bool isCustomRoute { get { return CustomRouteData != null; } }
        public CustomRouteData CustomRouteData { get; set; }

        public MountainGroup MountainGroup { get; set; }

        public RoutePoint StartingPoint { get; set; }
        public RoutePoint EndingPoint { get; set; }

        private int GetPoints()
        {
            int pointsForDistance = (int)Math.Round(Length);
            int pointsForClimbs = (int)Math.Round(SumOfClimbs / 100f);

            return pointsForDistance + pointsForClimbs;
        }
    }
}
