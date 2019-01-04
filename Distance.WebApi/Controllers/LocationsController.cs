using System.Threading.Tasks;
using Distance.Models;
using Microsoft.AspNetCore.Mvc;

namespace Distance.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public sealed class LocationsController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public LocationsController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("{latitude}/{longitude}/{distance?}/{results?}")]
        public async Task<ActionResult<SearchResult>> Get(
            double latitude,
            double longitude,
            int? distance,
            int? results)
        {
            var locations = await _searchService
                                  .GetLocations(new Coordinate(latitude, longitude), distance, results)
                                  .ConfigureAwait(false);

            return locations;
        }
    }
}
