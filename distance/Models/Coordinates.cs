using System;

namespace Distance.Models
{
    public struct Coordinates
    {
        private const double EarthRadius = 180 * 60 * 1.1515 * 1609.344 / Math.PI;

        public readonly double Latitude;
        public readonly double Longitude;
        public readonly double[] Values;

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Values = new[] { latitude, longitude };
        }

        public double Distance(Coordinates other)
        {
            var lat1 = Latitude;
            var long1 = Longitude;
            var lat2 = other.Latitude;
            var long2 = other.Longitude;

            var rLat1 = Math.PI * lat1 / 180;
            var rLat2 = Math.PI * lat2 / 180;
            var rTheta = Math.PI * (long2 - long1) / 180;
            var dist = Math.Sin(rLat1) * Math.Sin(rLat2) + Math.Cos(rLat1) * Math.Cos(rLat2) * Math.Cos(rTheta);

            return Math.Acos(dist) * EarthRadius;
        }

        public override string ToString()
        {
            return $"{Latitude}, {Longitude}";
        }
    }
}
