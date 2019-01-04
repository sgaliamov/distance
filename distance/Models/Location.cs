﻿namespace Distance.Models
{
    public struct Location
    {
        public string Address { get; }
        public Coordinates Coordinates { get; }

        public Location(string address, Coordinates coordinates)
        {
            Address = address;
            Coordinates = coordinates;
        }
    }
}
