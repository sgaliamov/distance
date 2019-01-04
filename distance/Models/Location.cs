namespace Distance.Models
{
    public struct Location
    {
        public string Address { get; }
        public Coordinates Coordinates { get; }

        public Location(Coordinates coordinates, string address)
        {
            Address = address;
            Coordinates = coordinates;
        }
    }
}
