namespace Distance.Models
{
    public sealed class SearchResult
    {
        public LocationAddress[] Locations { get; set; }
    }

    public sealed class LocationAddress
    {
        public string Address { get; set; }
        public dynamic Coordinate { get; set; }
    }
}
