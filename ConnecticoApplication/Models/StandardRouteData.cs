using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public enum Difficulty
    {
        Easy,
        Moderate,
        Hard,
        Extreme
    }

    public class StandardRouteData
    {
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int WalkingTime { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Difficulty Difficulty { get; set; }
    }
}
