using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnecticoApplication.Models
{
    public enum BadgeType
    {
        IntoMountains,
        Popular,
        Small,
        Big,
        ForPersistance
    }

    public class Badge
    {
        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public BadgeType Type { get; set; }
    }
}
