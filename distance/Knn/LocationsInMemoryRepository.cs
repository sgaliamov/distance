using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distance.DataAccess.Entities;
using Distance.Knn.KdTree;

namespace Distance.Knn
{
    public sealed class LocationsInMemoryRepository : ILocationsRepository
    {
        private readonly List<Location> _locations = new List<Location>();
        private SphereSearch<Location> _search;

        public Task<LocationEntity[]> GetLocations(double latitude, double longitude, int? maxDistance, int? maxResults)
        {
            BuildIfChanged();

            var result = _search.LookupDistance(
                                    new Location(latitude, longitude, null),
                                    maxResults ?? int.MaxValue,
                                    maxDistance ?? double.MaxValue)
                                .Select(x => new LocationEntity(x.Item1.Address, x.Item1.Latitude, x.Item1.Longitude, x.Item2))
                                .ToArray();

            return Task.FromResult(result);
        }

        public Task<long> AddLocation(double latitude, double longitude, string address)
        {
            _search = null;

            _locations.Add(new Location(latitude, longitude, address));

            return Task.FromResult(_locations.LongCount());
        }

        private void BuildIfChanged()
        {
            if (_search == null)
            {
                _search = new SphereSearch<Location>(_locations, c => Cartesian.FromSpherical(c.Latitude, c.Longitude));
            }
        }

        private struct Location
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
        }
    }
}
