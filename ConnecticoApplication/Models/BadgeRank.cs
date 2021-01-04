
namespace ConnecticoApplication.Models
{
    public class BadgeRank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Quota { get; set; }

        public Badge Badge { get; set; }
    }
}
