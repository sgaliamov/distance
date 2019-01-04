using System.Linq;
using System.Threading.Tasks;
using Distance.KdTree;
using Distance.Models;
using FluentAssertions;
using Xunit;

namespace Distance.Tests
{
    public sealed class LocationsInMemoryRepositoryTests : LocationsRepositoryTests
    {
        [Fact]
        public async Task Nearest_In_Corners()
        {
            await Repository.AddLocation(new Coordinates(89, -179), "upper left").ConfigureAwait(false);
            await Repository.AddLocation(new Coordinates(89, 179), "upper right").ConfigureAwait(false);
            await Repository.AddLocation(new Coordinates(-89, -179), "bottom left").ConfigureAwait(false);
            await Repository.AddLocation(new Coordinates(-89, 179), "bottom right").ConfigureAwait(false);

            var locations = await Repository.GetLocations(new Coordinates(88, -178), 150_000, null).ConfigureAwait(false);

            var expectation = new[] { "upper left", "bottom right", "upper right", "bottom left" };

            locations.Select(x => x.Address)
                     .Should()
                     .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task Nearest_On_Equator()
        {
            await Repository.AddLocation(new Coordinates(0, -179), "left").ConfigureAwait(false);

            var locations = await Repository.GetLocations(new Coordinates(0, 1), 300_000, null).ConfigureAwait(false);
        }

        protected override ILocationsRepository Repository { get; } = new LocationsInMemoryRepository();
    }
}
