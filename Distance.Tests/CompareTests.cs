using System;
using System.Linq;
using System.Threading.Tasks;
using Distance.BruteForce;
using Distance.KdTree;
using Distance.Models;
using FluentAssertions;
using Xunit;

namespace Distance.Tests
{
    public sealed class CompareTests
    {
        public CompareTests()
        {
            var locations = Enumerable
                            .Range(0, 1_000_000)
                            .AsParallel()
                            .Select(x => new Location(RandomCoordinates(), "dummy"))
                            .ToArray();

            _bruteRepository.SetLocations(locations);
            _inMemoryRepository.SetLocations(locations);
            _inMemoryRepository.Build();
        }

        [Fact]
        public async Task Compare_Results()
        {
            var target = RandomCoordinates();

            var brute = await _bruteRepository.GetLocations(target, 1_000_000, 100).ConfigureAwait(false);
            var kdTree = await _inMemoryRepository.GetLocations(target, 1_000_000, 100).ConfigureAwait(false);

            brute.Should().BeEquivalentTo(kdTree, options => options
                                                             .WithStrictOrdering()
                                                             .ComparingByMembers<Coordinates>()
                                                             .ComparingByMembers<LocationDistance>());
        }

        private readonly Random _random = new Random();
        private readonly LocationsInMemoryRepository _inMemoryRepository = new LocationsInMemoryRepository();
        private readonly LocationsBruteRepository _bruteRepository = new LocationsBruteRepository();

        private Coordinates RandomCoordinates()
        {
            return new Coordinates(_random.Next(-90, 91), _random.Next(-180, 181));
        }
    }
}
