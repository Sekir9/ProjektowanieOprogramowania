using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public DateTime DateOfPassing { get; set; }
        public bool Verified { get; set; }

        public Route Route { get; set; }
    }
}
