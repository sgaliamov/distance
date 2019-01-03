using Distance.DataAccess.Entities;

namespace Distance.Models
{
    public sealed class SearchResult
    {
        public LocationEntity[] Locations { get; set; }
    }
}
