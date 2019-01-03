using System;
using System.Threading.Tasks;
using System.Transactions;
using Distance.DataAccess;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Distance.Tests
{
    public sealed class LocationsRepositoryTests : IDisposable
    {
        public LocationsRepositoryTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var factory = new SqlConnectionFactory(configuration);

            _repository = new LocationsRepository(factory);

            _transactionScope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Dispose()
        {
            _transactionScope.Dispose();
        }

        [Fact]
        public async Task Test1()
        {
            var result = await _repository.CreateLocation(52.3546274, 4.8285836, "Amsterdam").ConfigureAwait(false);

            result.Should().BeGreaterThan(0);
        }

        private readonly LocationsRepository _repository;
        private readonly TransactionScope _transactionScope;
    }
}
