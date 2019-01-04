using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Distance.Models;

namespace Distance.Sql
{
    public sealed class LocationsSqlRepository : ILocationsRepository
    {
        private const string GetLocationsStoredProcedure = "[dbo].[GetLocations]";
        private const string InsertLocationsStoredProcedure = "[dbo].[InsertLocation]";

        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public LocationsSqlRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Location[]> GetLocations(
            double latitude,
            double longitude,
            int? maxDistance,
            int? maxResults)
        {
            using (var connection = _sqlConnectionFactory.GetConnection())
            {
                var locations = await connection.QueryAsync<Location>(
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

        public async Task<long> AddLocation(double latitude, double longitude, string address)
        {
            using (var connection = _sqlConnectionFactory.GetConnection())
            {
                return await connection.ExecuteScalarAsync<long>(
                                           InsertLocationsStoredProcedure,
                                           new
                                           {
                                               Longitude = longitude,
                                               Latitude = latitude,
                                               Address = address
                                           },
                                           commandType: CommandType.StoredProcedure)
                                       .ConfigureAwait(false);
            }
        }
    }
}
