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
        private const int Count = 10_000;

        public CompareTests()
        {
            var locations = Enumerable
                            .Range(0, Count)
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

            var brute = await _bruteRepository.GetLocations(target, 300_000, null).ConfigureAwait(false);
            var kdTree = await _inMemoryRepository.GetLocations(target, 300_000, null).ConfigureAwait(false);

            kdTree.Should()
                  .BeEquivalentTo(
                      brute,
                      options => options
                                 .WithStrictOrdering()
                                 .ComparingByMembers<Coordinates>()
                                 .Excluding(x => x.Coordinates.Values)
                                 .ComparingByMembers<LocationDistance>());

#if DEBUG
            KdTree.KdTree.Counter.Should().BeLessThan(Count);
#endif
        }

        private readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private readonly LocationsInMemoryRepository _inMemoryRepository = new LocationsInMemoryRepository();
        private readonly LocationsBruteRepository _bruteRepository = new LocationsBruteRepository();

        private Coordinates RandomCoordinates()
        {
            return new Coordinates(_random.Next(-90, 91), _random.Next(-180, 181));
        }
    }
}
