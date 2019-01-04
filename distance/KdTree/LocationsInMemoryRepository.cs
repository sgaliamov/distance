using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distance.DataAccess.Entities;

namespace Distance.KdTree
{
    public sealed class LocationsInMemoryRepository : ILocationsRepository
    {
        private readonly List<Location> _locations = new List<Location>();
        private Node _root;

        public Task<LocationEntity[]> GetLocations(double latitude, double longitude, int? maxDistance, int? maxResults)
        {
            BuildIfChanged();

            var result = _root.Nearest(new Point(-1, latitude, longitude), maxDistance ?? double.MaxValue)
                              .OrderBy(x => x.Distance)
                              .Select(x => new LocationEntity(
                                  _locations[x.Node.Position.Id].Address,
                                  x.Node.Position.Coordinates[0],
                                  x.Node.Position.Coordinates[1],
                                  x.Distance));

            if (maxResults.HasValue)
            {
                result = result.Take(maxResults.Value);
            }

            return Task.FromResult(result.ToArray());
        }

        public Task<long> AddLocation(double latitude, double longitude, string address)
        {
            _root = null;

            _locations.Add(new Location(latitude, longitude, address));

            return Task.FromResult(_locations.LongCount());
        }

        private void BuildIfChanged()
        {
            if (_root == null)
            {
                var points = _locations
                             .Select((x, i) => new Point(i, x.Latitude, x.Longitude))
                             .ToArray();

                _root = KdTree.Build(points);
            }
        }

        private struct Location : IEquatable<Location>
        {
            public string Address { get; }
            public double Latitude { get; }
            public double Longitude { get; }

            public Location(double latitude, double longitude, string address)
            {
                Address = address;
                Latitude = latitude;
                Longitude = longitude;
            }

            public bool Equals(Location other)
            {
                return string.Equals(Address, other.Address)
                       && Latitude.Equals(other.Latitude)
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
                    var hashCode = Address != null ? Address.GetHashCode() : 0;
                    hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                    hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}
