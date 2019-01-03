using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Distance.DataAccess.Entities;

namespace Distance.DataAccess
{
    public sealed class LocationsRepository : ILocationsRepository
    {
        private const string GetLocationsStoredProcedure = "[dbo].[GetLocations]";

        private readonly IDbConnection _connection;

        public LocationsRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<LocationEntity[]> GetLocations(
            double longitude,
            double latitude,
            int? maxDistance,
            int? maxResults)
        {
            var locations = await _connection
                                  .QueryAsync<LocationEntity>(
                                      GetLocationsStoredProcedure,
                                      new
                                      {
                                          Longitude = longitude,
                                          Latitude = latitude,
                                          Count = maxResults,
                                          Distance = maxDistance
                                      },
                                      commandType: CommandType.StoredProcedure)
                                  .ConfigureAwait(false);

            return locations.ToArray();
        }
    }
}
