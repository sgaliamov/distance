using System.Threading.Tasks;
using Distance.DataAccess;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Distance.Tests
{
    public sealed class LocationsRepositoryTests
    {
        public LocationsRepositoryTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var factory = new SqlConnectionFactory(configuration);

            _repository = new LocationsRepository(factory);
        }

        [Fact]
        public async Task Test1()
        {
            var result = await _repository.GetLocations(0, 0, 100000, 10).ConfigureAwait(false);

            result.Should().HaveCount(10);
        }

        private readonly LocationsRepository _repository;
    }
}
