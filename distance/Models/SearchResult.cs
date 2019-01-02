namespace Distance.Models
{
    public sealed class SearchResult
    {
        public LocationAddress[] Locations { get; set; }
    }

    public sealed class LocationAddress
    {
        public string Address { get; set; }
        public double Distance { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
