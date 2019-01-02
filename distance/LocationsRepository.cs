using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Distance.Models;

namespace Distance
{
    public sealed class LocationsRepository
    {
        private const string GetLocationsStoredProcedure = "[dbo].[GetLocations]";

        private readonly IDbConnection _connection;

        public LocationsRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<SearchResult> GetLocations(Location location, int? maxDistance, int? maxResults)
        {
            var locations = await _connection
                                  .QueryAsync<LocationAddress>(
                                      GetLocationsStoredProcedure,
                                      new
                                      {
                                          location.Longitude,
                                          location.Latitude,
                                          Count = maxResults,
                                          Distance = maxDistance
                                      })
                                  .ConfigureAwait(false);

            return new SearchResult
            {
                Locations = locations.ToArray()
            };
        }
    }
}
