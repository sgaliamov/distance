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
        private const int MaxDistance = 600_000;
        private const int MaxResults = 10;
        private const int Times = 10;
        private readonly LocationsBruteRepository _bruteRepository = new LocationsBruteRepository();
        private readonly LocationsInMemoryRepository _inMemoryRepository = new LocationsInMemoryRepository();
        private readonly Random _random = new Random();
        private LocationsSqlRepository _sqlRepository;

        [GlobalSetup]
        public void Setup()
        {
            int GetCount(SqlConnectionFactory sqlConnectionFactory)
            {
                using (var connection = sqlConnectionFactory.GetConnection())
                {
                    return connection.ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[Locations]");
                }
            }

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var factory = new SqlConnectionFactory(configuration);
            _sqlRepository = new LocationsSqlRepository(factory);

            var locations = Enumerable
                            .Range(0, GetCount(factory))
                            .Select(x => new Location(RandomCoordinates(), "dummy"))
                            .ToArray();

            _bruteRepository.SetLocations(locations);
            _inMemoryRepository.SetLocations(locations);
            _inMemoryRepository.Build();
        }

        [Benchmark(Baseline = true)]
        public void SqlServer()
        {
            for (var i = 0; i < Times; i++)
            {
                Task.WaitAll(_sqlRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
            }
        }

        [Benchmark]
        public void InMemory()
        {
            for (var i = 0; i < Times; i++)
            {
                Task.WaitAll(_inMemoryRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
            }
        }

        [Benchmark]
        public void BruteForce()
        {
            for (var i = 0; i < Times; i++)
            {
                Task.WaitAll(_bruteRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
            }
        }

        private Coordinates RandomCoordinates()
        {
            return new Coordinates(_random.Next(-90, 91), _random.Next(-180, 181));
        }
    }
}

/*
     Method |       Mean |     Error |    StdDev |     Median | Rank |
----------- |-----------:|----------:|----------:|-----------:|-----:|
   InMemory |   912.1 us |  20.50 us |  57.81 us |   893.7 us |    1 |
 BruteForce | 4,211.3 us | 107.00 us | 305.28 us | 4,127.7 us |    2 |
 */
