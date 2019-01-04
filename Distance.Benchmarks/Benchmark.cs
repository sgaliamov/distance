using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Dapper;
using Distance.BruteForce;
using Distance.KdTree;
using Distance.Models;
using Distance.Sql;
using Microsoft.Extensions.Configuration;

namespace Distance.Benchmarks
{
    [MedianColumn]
    [RankColumn]
    public class Benchmark
    {
        protected internal const int MaxDistance = 1_000_000;
        protected internal const int MaxResults = 10;
        private readonly LocationsBruteRepository _bruteRepository = new LocationsBruteRepository();
        private readonly LocationsInMemoryRepository _inMemoryRepository = new LocationsInMemoryRepository();
        private readonly Random _random = new Random();
        private LocationsSqlRepository _sqlRepository;

        [GlobalSetup]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var factory = new SqlConnectionFactory(configuration);

            _sqlRepository = new LocationsSqlRepository(factory);

            using (var connection = factory.GetConnection())
            {
                var locations = connection
                                .Query<LocationEntity>(
                                    "SELECT TOP (10000) [Address], [Coordinate].Lat AS [Latitude], [Coordinate].Long AS [Longitude] "
                                    + "FROM [dbo].[Locations]")
                                .ToArray();

                foreach (var location in locations)
                {
                    _bruteRepository.AddLocation(new Coordinates(location.Latitude, location.Longitude), location.Address);
                    _inMemoryRepository.AddLocation(new Coordinates(location.Latitude, location.Longitude), location.Address);
                }
            }

            Task.WaitAll(_inMemoryRepository.GetLocations(new Coordinates(0, 0), 0, 1));
        }

        [Benchmark(Baseline = true)]
        public void ImMemory()
        {
            Task.WhenAll(_inMemoryRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
        }

        [Benchmark]
        public void BruteForce()
        {
            Task.WhenAll(_bruteRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
        }

        [Benchmark]
        public void SqlServer()
        {
            Task.WhenAll(_sqlRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
        }

        private Coordinates RandomCoordinates()
        {
            return new Coordinates(_random.Next(-90, 90), _random.Next(-180, 180));
        }
    }
}
