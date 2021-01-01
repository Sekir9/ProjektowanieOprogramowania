using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public class MountainGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public int? LeadersWithPermissionsCount { get; set; }
        public int? RoutesInGroupCount { get; set; }
    }
}
