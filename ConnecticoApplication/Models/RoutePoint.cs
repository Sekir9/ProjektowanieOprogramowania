using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public class RoutePoint
    {
        public const int NAME_MAX_LENGTH = 50;
        public const int DESCRIPTION_MAX_LENGTH = 500;
        public const string CORDINATES_REGEX = "[0-9][0-9]° [0-9][0-9]' [0-9][0-9]\" [NS] [0-9][0-9]° [0-9][0-9]' [0-9][0-9]\" [EW]";

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Cordinates { get; set; }
        public int Height { get; set; }
        public int UsedInRoutes { get; set; }
    }
}
