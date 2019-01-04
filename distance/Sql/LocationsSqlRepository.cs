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
            Coordinates coordinates,
            int? maxDistance,
            int? maxResults)
        {
            using (var connection = _sqlConnectionFactory.GetConnection())
            {
                var locations = await connection.QueryAsync<Location>(
                                                    GetLocationsStoredProcedure,
                                                    new
                                                    {
                                                        coordinates.Longitude,
                                                        coordinates.Latitude,
                                                        Count = maxResults,
                                                        Distance = maxDistance
                                                    },
                                                    commandType: CommandType.StoredProcedure)
                                                .ConfigureAwait(false);

                return locations.ToArray();
            }
        }

        public async Task<long> AddLocation(Coordinates coordinates, string address)
        {
            using (var connection = _sqlConnectionFactory.GetConnection())
            {
                return await connection
                             .ExecuteScalarAsync<long>(
                                 InsertLocationsStoredProcedure,
                                 new
                                 {
                                     coordinates.Longitude,
                                     coordinates.Latitude,
                                     Address = address
                                 },
                                 commandType: CommandType.StoredProcedure)
                             .ConfigureAwait(false);
            }
        }
    }
}
