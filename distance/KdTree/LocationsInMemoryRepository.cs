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

        public Task<Location[]> GetLocations(double latitude, double longitude, int? maxDistance, int? maxResults)
        {
            BuildIfChanged();

            var result = _tree.Nearest(new Coordinates(latitude, longitude), maxDistance ?? double.MaxValue).AsEnumerable();

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

            _locations.Add(new Location(address, new Coordinates(latitude, longitude), 0));

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
