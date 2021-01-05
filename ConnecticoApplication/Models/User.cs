using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsAdmin { get; set; }

        public bool isTurist { get; set; }
        public TuristData TuristData { get; set; }

        public bool isLeader { get; set; }
    }
}
