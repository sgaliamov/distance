namespace Distance.Models
{
    public struct LocationDistance
    {
        public readonly string Address;
        public readonly double Distance;
        public readonly Coordinates Coordinates;

        public LocationDistance(string address, Coordinates coordinates, double distance)
        {
            Address = address;
            Distance = distance;
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return $"({Coordinates})";
        }
    }
}
