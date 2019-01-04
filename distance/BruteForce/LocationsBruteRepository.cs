using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distance.Models;

namespace Distance.BruteForce
{
    public sealed class LocationsBruteRepository : ILocationsRepository
    {
        private readonly ICollection<Location> _locations = new LinkedList<Location>();

        public Task<LocationDistance[]> GetLocations(Coordinates coordinates, int? maxDistance, int? maxResults)
        {
            var result = new LinkedList<LocationDistance>();

            foreach (var location in _locations)
            {
                var distance = location.Coordinates.Distance(coordinates);

                if (maxDistance.HasValue)
                {
                    if (distance <= maxDistance.Value)
                    {
                        result.AddLast(new LocationDistance(location.Address, location.Coordinates, distance));        
                    }
                }
                else
                {
                    result.AddLast(new LocationDistance(location.Address, location.Coordinates, distance));
                }
            }

            var enumerable = result.OrderBy(x => x.Distance).AsEnumerable();
            if (maxResults.HasValue)
            {
                enumerable = enumerable.Take(maxResults.Value);
            }

            return Task.FromResult(enumerable.ToArray());
        }

        public Task<long> AddLocation(Coordinates coordinates, string address)
        {
            _locations.Add(new Location(address, coordinates));

            return Task.FromResult(_locations.LongCount());
        }
    }
}
