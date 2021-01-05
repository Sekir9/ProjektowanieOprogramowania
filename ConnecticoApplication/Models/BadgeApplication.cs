using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public class BadgeApplication
    {
        public const int DESCRIPTION_MAX_LENGTH = 500;

        public int Id { get; set; }
        public DateTime? AwardDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VerificationStatus Status { get; set; }
        public string Description { get; set; }

        public int LeaderId { get; set; }
        public int TuristId { get; set; }

        public BadgeRank Rank { get; set; }

        public TuristData Turist { get; set; }

        public ICollection<Tour> Tours { get; set; }
    }
}
