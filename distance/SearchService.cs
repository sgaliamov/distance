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

        public async Task<SearchResult> GetLocations(Coordinates coordinates, int? maxDistance, int? maxResults)
        {
            var locations = await _locationsRepository
                                  .GetLocations(coordinates.Latitude, coordinates.Longitude, maxDistance, maxResults)
                                  .ConfigureAwait(false);

            return new SearchResult
            {
                Locations = locations
            };
        }
    }
}
