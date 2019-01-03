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

        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public LocationsRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<LocationEntity[]> GetLocations(
            double longitude,
            double latitude,
            int? maxDistance,
            int? maxResults)
        {
            using (var connection = _sqlConnectionFactory.GetConnection())
            {
                var locations = await connection
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
}
