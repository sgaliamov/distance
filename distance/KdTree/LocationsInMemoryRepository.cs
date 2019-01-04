using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distance.Models;

namespace Distance.KdTree
{
    public sealed class LocationsInMemoryRepository : ILocationsRepository
    {
        private readonly List<Location> _locations = new List<Location>();
        private KdTree _tree;

        public Task<Models.Location[]> GetLocations(double latitude, double longitude, int? maxDistance, int? maxResults)
        {
            BuildIfChanged();

            var result = _tree.Nearest(new Point(-1, latitude, longitude), maxDistance ?? double.MaxValue)
                              .Select(x => new Models.Location(
                                  _locations[x.Node.Position.Id].Address,
                                  x.Node.Position.Coordinates[0],
                                  x.Node.Position.Coordinates[1],
                                  x.Distance));

            if (maxResults.HasValue)
            {
                result = result.Take(maxResults.Value);
            }

            result = result.OrderBy(x => x.Distance);

            return Task.FromResult(result.ToArray());
        }

        public Task<long> AddLocation(double latitude, double longitude, string address)
        {
            _tree = null;

            _locations.Add(new Location(latitude, longitude, address));

            return Task.FromResult(_locations.LongCount());
        }

        private void BuildIfChanged()
        {
            if (_tree != null) { return; }

            var points = _locations
                         .Select((x, i) => new Point(i, x.Latitude, x.Longitude))
                         .ToArray();

            _tree = new KdTree();

            _tree.Build(points);
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
