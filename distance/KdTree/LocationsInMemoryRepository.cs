using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distance.Models;

namespace Distance.KdTree
{
    public sealed class LocationsInMemoryRepository : ILocationsRepository
    {
        private ICollection<Location> _locations = new List<Location>();
        private KdTree _tree;

        public Task<LocationDistance[]> GetLocations(Coordinates coordinates, int? maxDistance, int? maxResults)
        {
            Build();

            var result = _tree.Nearest(coordinates, maxDistance ?? double.MaxValue).AsParallel();

            if (maxResults.HasValue)
            {
                result = result.Take(maxResults.Value);
            }

            result = result.OrderBy(x => x.Distance);

            return Task.FromResult(result.ToArray());
        }

        public Task<long> AddLocation(Coordinates coordinates, string address)
        {
            _tree = null;

            _locations.Add(new Location(coordinates, address));

            return Task.FromResult(_locations.LongCount());
        }

        public void SetLocations(ICollection<Location> locations)
        {
            _locations = locations;
            _tree = null;
        }

        public void Build()
        {
            if (_tree != null) { return; }

            _tree = new KdTree();

            _tree.Build(_locations);
        }
    }
}
