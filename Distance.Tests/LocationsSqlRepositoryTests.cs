using System;
using System.Threading.Tasks;
using System.Transactions;
using Distance.Models;
using Distance.Sql;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Distance.Tests
{
    public sealed class LocationsSqlRepositoryTests : LocationsRepositoryTests, IDisposable
    {
        private readonly TransactionScope _transactionScope;

        public LocationsSqlRepositoryTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var factory = new SqlConnectionFactory(configuration);

            Repository = new LocationsSqlRepository(factory);

            _transactionScope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                TransactionScopeAsyncFlowOption.Enabled);
        }

        
       

        protected override ILocationsRepository Repository { get; }

        public void Dispose()
        {
            _transactionScope.Dispose();
        }
    }
}
