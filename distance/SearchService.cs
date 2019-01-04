using System.Threading.Tasks;
using Distance.Models;

namespace Distance
{
    public sealed class SearchService : ISearchService
    {
        private readonly ILocationsRepository _locationsRepository;

        public SearchService(ILocationsRepository locationsRepository)
        {
            _locationsRepository = locationsRepository;
        }

        public async Task<SearchResult> GetLocations(Coordinate coordinate, int? maxDistance, int? maxResults)
        {
            var locations = await _locationsRepository
                                  .GetLocations(coordinate.Latitude, coordinate.Longitude, maxDistance, maxResults)
                                  .ConfigureAwait(false);

            return new SearchResult
            {
                Locations = locations
            };
        }
    }
}
