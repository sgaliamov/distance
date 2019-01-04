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
            await Repository.AddLocation(new Coordinates(0, 0), "center").ConfigureAwait(false);

            var locations = await Repository.GetLocations(new Coordinates(88, -178), 1_000_000, null).ConfigureAwait(false);

            var expectation = new[] { "upper left", "upper right", "bottom right", "bottom left" };

            locations.Select(x => x.Address)
                     .Should()
                     .BeEquivalentTo(expectation, opt => opt.WithStrictOrdering());
        }

        protected override ILocationsRepository Repository { get; } = new LocationsInMemoryRepository();
    }
}
