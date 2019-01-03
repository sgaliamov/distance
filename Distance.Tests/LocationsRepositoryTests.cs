using System.Data.SqlClient;
using System.Threading.Tasks;
using Distance.DataAccess;
using FluentAssertions;
using Xunit;

namespace Distance.Tests
{
    public class LocationsRepositoryTests
    {
        [Fact]
        public async Task Test1()
        {
            using (var connection = new SqlConnection("Data Source=.;Integrated Security=True;Database=LocationsDb;"))
            {
                var repository = new LocationsRepository(connection);

                var result = await repository.GetLocations(0, 0, 100000, 10).ConfigureAwait(false);

                result.Should().HaveCount(10);
            }
        }
    }
}
