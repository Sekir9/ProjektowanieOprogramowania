using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }

        public int TuristId { get; set; }

        public ICollection<Entry> Entries { get; set; }
    }
}
