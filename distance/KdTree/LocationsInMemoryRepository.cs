using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distance.Models;

namespace Distance.KdTree
{
    public sealed class LocationsInMemoryRepository : ILocationsRepository
    {
        private readonly ICollection<Location> _locations = new LinkedList<Location>();
        private KdTree _tree;

        public Task<Location[]> GetLocations(Coordinates coordinates, int? maxDistance, int? maxResults)
        {
            BuildIfChanged();

            var result = _tree.Nearest(coordinates, maxDistance ?? double.MaxValue).AsEnumerable();

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

            _locations.Add(new Location(address, coordinates, 0));

            return Task.FromResult(_locations.LongCount());
        }

        private void BuildIfChanged()
        {
            if (_tree != null) { return; }

            _tree = new KdTree();

            _tree.Build(_locations);
        }
    }
}
