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
        [Fact]
        public async Task Compare_Results()
        {
            var locations = Enumerable
                            .Range(0, Count)
                            .Select(x => new Location(RandomCoordinates(), "dummy"))
                            .ToArray();

            _bruteRepository.SetLocations(locations);
            _inMemoryRepository.SetLocations(locations);

            var target = new Coordinates(0, 0);

            await Test(target, 600_000).ConfigureAwait(false);

#if DEBUG
            KdTree.KdTree.Counter.Should().BeLessThan(Count);
#endif
        }

        [Fact]
        public async Task Inner_Nodes_Intersection()
        {
            var target = new Coordinates(5, 5);
            var locations = new[]
            {
                new Location(new Coordinates(4, 4), "dummy"),
                new Location(new Coordinates(5, 3), "dummy"),
                new Location(new Coordinates(2, 2), "dummy"),
                new Location(new Coordinates(6, 2), "dummy"),

                new Location(new Coordinates(3, 6), "dummy"),
                new Location(new Coordinates(4, 7), "dummy"),
                new Location(new Coordinates(7, 8), "dummy")
            };

            _bruteRepository.SetLocations(locations);
            _inMemoryRepository.SetLocations(locations);

            await Test(target, 200_000).ConfigureAwait(false);
        }

        private async Task Test(Coordinates target, int maxDistance)
        {
            var brute = await _bruteRepository.GetLocations(target, maxDistance, null).ConfigureAwait(false);
            var kdTree = await _inMemoryRepository.GetLocations(target, maxDistance, null).ConfigureAwait(false);

            kdTree.Should()
                  .BeEquivalentTo(
                      brute,
                      options => options
                                 .WithStrictOrdering()
                                 .ComparingByMembers<Coordinates>()
                                 .Excluding(x => x.Coordinates.Values)
                                 .ComparingByMembers<LocationDistance>());
        }

        private const int Count = 10_000;

        private readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private readonly LocationsInMemoryRepository _inMemoryRepository = new LocationsInMemoryRepository();
        private readonly LocationsBruteRepository _bruteRepository = new LocationsBruteRepository();

        private Coordinates RandomCoordinates()
        {
            return new Coordinates(_random.Next(-90, 91), _random.Next(-180, 181));
        }
    }
}
