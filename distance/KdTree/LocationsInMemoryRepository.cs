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
            BuildTree();

            var result = _tree.Nearest(coordinates, maxDistance ?? double.MaxValue).AsParallel();

            result = result.OrderBy(x => x.Distance).ThenBy(x => x.Coordinates.Latitude).ThenBy(x => x.Coordinates.Longitude);

            if (maxResults.HasValue)
            {
                result = result.Take(maxResults.Value);
            }

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

        public void BuildTree()
        {
            if (_tree != null) { return; }

            _tree = new KdTree();

            _tree.Build(_locations);
        }
    }
}
