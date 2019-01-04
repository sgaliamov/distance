namespace Distance.Models
{
    public struct Location
    {
        public string Address { get; }
        public double Distance { get; }
        public Coordinates Coordinates { get; }

        public Location(string address, Coordinates coordinates, double distance)
        {
            Address = address;
            Distance = distance;
            Coordinates = coordinates;
        }
    }
}
