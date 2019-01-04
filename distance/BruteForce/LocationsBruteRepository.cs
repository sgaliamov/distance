using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distance.Models;

namespace Distance.BruteForce
{
    public sealed class LocationsBruteRepository : ILocationsRepository
    {
        private ICollection<Location> _locations = new List<Location>();

        public Task<LocationDistance[]> GetLocations(Coordinates coordinates, int? maxDistance, int? maxResults)
        {
            var query = _locations
                        .AsParallel()
                        .Select(location => new LocationDistance(
                            location.Address,
                            location.Coordinates,
                            location.Coordinates.Distance(coordinates)));

            if (maxDistance.HasValue)
            {
                query = query.Where(x => x.Distance <= maxDistance.Value);
            }

            query = query.OrderBy(x => x.Distance);

            if (maxResults.HasValue)
            {
                query = query.Take(maxResults.Value);
            }

            return Task.FromResult(query.ToArray());
        }

        public Task<long> AddLocation(Coordinates coordinates, string address)
        {
            _locations.Add(new Location(coordinates, address));

            return Task.FromResult(_locations.LongCount());
        }

        public void SetLocations(ICollection<Location> locations)
        {
            _locations = locations;
        }
    }
}
