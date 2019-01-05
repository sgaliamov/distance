namespace Distance.Models
{
    public struct Location
    {
        public readonly string Address;
        public readonly Coordinates Coordinates;

        public Location(Coordinates coordinates, string address)
        {
            Address = address;
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return $"{Address} ({Coordinates})";
        }
    }
}
