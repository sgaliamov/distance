using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Distance.Models;
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

                var result = await repository.GetLocations(new Location(0, 0), null, 10).ConfigureAwait(false);

                result.Locations.Should().HaveCount(10);
            }
        }
    }
}
