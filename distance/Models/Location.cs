﻿using System;

namespace Distance.Models
{
    public struct Location : IEquatable<Location>
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Distance(Location location)
        {
            var rLat1 = Math.PI * Latitude / 180;
            var rLat2 = Math.PI * location.Latitude / 180;
            var rTheta = Math.PI * (location.Longitude - Longitude) / 180;
            var dist = Math.Sin(rLat1) * Math.Sin(rLat2) + Math.Cos(rLat1) * Math.Cos(rLat2) * Math.Cos(rTheta);

            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }

        public bool Equals(Location other)
        {
            return Latitude.Equals(other.Latitude)
                   && Longitude.Equals(other.Longitude);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Location other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Latitude + ", " + Longitude;
        }
    }
}
