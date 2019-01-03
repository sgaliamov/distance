using System;
using System.Threading.Tasks;
using System.Transactions;
using Distance.DataAccess;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Distance.Tests
{
    public sealed class LocationsSqlRepositoryTests : IDisposable
    {
        public LocationsSqlRepositoryTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var factory = new SqlConnectionFactory(configuration);

            _repository = new LocationsSqlRepository(factory);

            _transactionScope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Dispose()
        {
            _transactionScope.Dispose();
        }

        [Fact]
        public async Task Haarlem_Is_Close_To_Amsterdam()
        {
            await _repository.CreateLocation(AmsterdamLongitude, AmsterdamLatitude, Amsterdam).ConfigureAwait(false);
            await _repository.CreateLocation(NewYorkLongitude, NewYorkLatitude, NewYork).ConfigureAwait(false);

            var result = await _repository.GetLocations(HaarlemLongitude, HaarlemLatitude, 30000, null).ConfigureAwait(false);

            result.Should().HaveCount(1);
            result[0].Address.Should().Be(Amsterdam);
        }

        [Fact]
        public async Task Haarlem_Is_Not_To_Close_To_Amsterdam()
        {
            await _repository.CreateLocation(AmsterdamLongitude, AmsterdamLatitude, Amsterdam).ConfigureAwait(false);
            await _repository.CreateLocation(NewYorkLongitude, NewYorkLatitude, NewYork).ConfigureAwait(false);

            var result = await _repository.GetLocations(HaarlemLongitude, HaarlemLatitude, 10000, null).ConfigureAwait(false);

            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task Insert_Amsterdam()
        {
            var result = await _repository.CreateLocation(AmsterdamLongitude, AmsterdamLatitude, Amsterdam).ConfigureAwait(false);

            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Locations_Are_Presented_In_Sorted_Order()
        {
            await _repository.CreateLocation(AmsterdamLongitude, AmsterdamLatitude, Amsterdam).ConfigureAwait(false);
            await _repository.CreateLocation(NewYorkLongitude, NewYorkLatitude, NewYork).ConfigureAwait(false);
            await _repository.CreateLocation(HaarlemLongitude, HaarlemLatitude, Haarlem).ConfigureAwait(false);

            var result = await _repository.GetLocations(0, 0, null, null).ConfigureAwait(false);

            result.Should().HaveCount(3);
            result[0].Address.Should().Be(Amsterdam);
            result[1].Address.Should().Be(Haarlem);
            result[2].Address.Should().Be(NewYork);
        }

        [Fact]
        public async Task Only_Closest_Location_Is_Found()
        {
            await _repository.CreateLocation(AmsterdamLongitude, AmsterdamLatitude, Amsterdam).ConfigureAwait(false);
            await _repository.CreateLocation(NewYorkLongitude, NewYorkLatitude, NewYork).ConfigureAwait(false);

            var result = await _repository.GetLocations(HaarlemLongitude, HaarlemLatitude, null, 1).ConfigureAwait(false);

            result.Should().HaveCount(1);
            result[0].Address.Should().Be(Amsterdam);
        }

        private const string Amsterdam = "Amsterdam";
        private const double AmsterdamLongitude = 52.3546274;
        private const double AmsterdamLatitude = 4.8285836;
        private const string NewYork = "New York";
        private const double NewYorkLongitude = 40.6971494;
        private const double NewYorkLatitude = -74.2598643;
        private const string Haarlem = "Haarlem";
        private const double HaarlemLongitude = 52.3837711;
        private const double HaarlemLatitude = 4.5728393;

        private readonly LocationsSqlRepository _repository;
        private readonly TransactionScope _transactionScope;
    }
}
