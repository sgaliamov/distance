namespace Distance.Models
{
    public struct LocationDistance
    {
        public string Address { get; }
        public double Distance { get; }
        public Coordinates Coordinates { get; }

        public LocationDistance(string address, Coordinates coordinates, double distance)
        {
            Address = address;
            Distance = distance;
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return $"{Address} ({Coordinates}), {Distance}";
        }
    }
}
