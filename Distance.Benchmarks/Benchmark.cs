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
        private const int MaxDistance = 1_000_000;
        private const int MaxResults = 10;
        private const int Times = 100;
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

            Console.WriteLine("Start...");

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var factory = new SqlConnectionFactory(configuration);
            _sqlRepository = new LocationsSqlRepository(factory);

            var locations = Enumerable
                            .Range(0, GetCount(factory))
                            .AsParallel()
                            .Select(x => new Location(RandomCoordinates(), "dummy"))
                            .ToArray();

            _bruteRepository.SetLocations(locations);
            _inMemoryRepository.SetLocations(locations);

            Console.WriteLine("Build tree...");
            
            _inMemoryRepository.Build();

            Console.WriteLine("Run...");
        }

        [Benchmark(Baseline = true)]
        public void SqlServer()
        {
            for (var i = 0; i < Times; i++)
            {
                Task.WhenAll(_sqlRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
            }
        }

        [Benchmark]
        public void ImMemory()
        {
            for (var i = 0; i < Times; i++)
            {
                Task.WhenAll(_inMemoryRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
            }
        }

        [Benchmark]
        public void BruteForce()
        {
            for (var i = 0; i < Times; i++)
            {
                Task.WhenAll(_bruteRepository.GetLocations(RandomCoordinates(), MaxDistance, MaxResults));
            }
        }

        private Coordinates RandomCoordinates()
        {
            return new Coordinates(_random.Next(-90, 91), _random.Next(-180, 181));
        }
    }
}

/*
      Method |        Mean |      Error |    StdDev |    Median | Ratio | RatioSD | Rank |
----------- |------------:|-----------:|----------:|----------:|------:|--------:|-----:|
   ImMemory | 1,018.86 us | 20.9031 us | 46.320 us | 993.53 us |  1.00 |    0.00 |    3 |
 BruteForce |   780.72 us |  6.8431 us |  6.401 us | 779.71 us |  0.74 |    0.04 |    2 |
  SqlServer |    37.39 us |  0.7399 us |  1.236 us |  37.40 us |  0.04 |    0.00 |    1 |
 */

/*
     Method |          Mean |       Error |        StdDev |        Median |     Ratio | RatioSD | Rank |
----------- |--------------:|------------:|--------------:|--------------:|----------:|--------:|-----:|
  SqlServer |      4.193 ms |   0.0838 ms |     0.1693 ms |      4.172 ms |      1.00 |    0.00 |    1 |
   ImMemory | 43,105.675 ms | 857.2321 ms |   952.8112 ms | 42,806.964 ms | 10,268.02 |  439.45 |    3 |
 BruteForce | 23,487.017 ms | 500.7908 ms | 1,099.2480 ms | 23,313.587 ms |  5,623.95 |  329.36 |    2 |
 */