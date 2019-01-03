using System;

namespace Distance.DataAccess.Entities
{
    public struct LocationEntity : IEquatable<LocationEntity>
    {
        public string Address { get; }
        public double Distance { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        public LocationEntity(string address, double latitude, double longitude, double distance)
        {
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            Distance = distance;
        }

        public bool Equals(LocationEntity other)
        {
            return string.Equals(Address, other.Address)
                   && Distance.Equals(other.Distance)
                   && Latitude.Equals(other.Latitude)
                   && Longitude.Equals(other.Longitude);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is LocationEntity other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Address != null ? Address.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Distance.GetHashCode();
                hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Longitude.GetHashCode();

                return hashCode;
            }
        }
    }
}
