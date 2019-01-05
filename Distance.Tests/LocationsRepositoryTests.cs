using System.Threading.Tasks;
using Distance.Models;
using FluentAssertions;
using Xunit;

namespace Distance.Tests
{
    public abstract class LocationsRepositoryTests
    {
        [Fact]
        public async Task Haarlem_Is_Close_To_Amsterdam()
        {
            await Repository.AddLocation(AmsterdamCoordinates, Amsterdam).ConfigureAwait(false);
            await Repository.AddLocation(NewYorkCoordinates, NewYork).ConfigureAwait(false);

            var result = await Repository.GetLocations(HaarlemCoordinates, 20000, null).ConfigureAwait(false);

            result.Should().HaveCount(1);
            result[0].Address.Should().Be(Amsterdam);
            result[0].Coordinates.Should().BeEquivalentTo(AmsterdamCoordinates, opt => opt.ComparingByMembers<Coordinates>());
        }

        [Fact]
        public async Task Haarlem_Is_Not_To_Close_To_Amsterdam()
        {
            await Repository.AddLocation(AmsterdamCoordinates, Amsterdam).ConfigureAwait(false);
            await Repository.AddLocation(NewYorkCoordinates, NewYork).ConfigureAwait(false);

            var result = await Repository.GetLocations(HaarlemCoordinates, 10000, null).ConfigureAwait(false);

            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task Insert_Amsterdam()
        {
            var result = await Repository.AddLocation(AmsterdamCoordinates, Amsterdam).ConfigureAwait(false);

            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Locations_Are_Presented_In_Sorted_Order()
        {
            await Repository.AddLocation(AmsterdamCoordinates, Amsterdam).ConfigureAwait(false);
            await Repository.AddLocation(NewYorkCoordinates, NewYork).ConfigureAwait(false);
            await Repository.AddLocation(HaarlemCoordinates, Haarlem).ConfigureAwait(false);

            var result = await Repository.GetLocations(new Coordinates(0, 0), null, null).ConfigureAwait(false);

            result.Should().HaveCount(3);
            result[0].Address.Should().Be(Amsterdam);
            result[1].Address.Should().Be(Haarlem);
            result[2].Address.Should().Be(NewYork);
        }

        [Fact]
        public async Task Nearest_On_Equator()
        {
            await Repository.AddLocation(new Coordinates(0, -1), "one").ConfigureAwait(false);
            await Repository.AddLocation(new Coordinates(0, -179), "another").ConfigureAwait(false);

            var one = await Repository.GetLocations(new Coordinates(0, 1), 300_000, null).ConfigureAwait(false);
            one.Should().HaveCount(1);
            one[0].Address.Should().Be("one");

            var another = await Repository.GetLocations(new Coordinates(0, 179), 300_000, null).ConfigureAwait(false);
            another.Should().HaveCount(1);
            another[0].Address.Should().Be("another");
        }

        [Fact]
        public async Task Nearest_On_Poles()
        {
            await Repository.AddLocation(new Coordinates(89, -1), "one").ConfigureAwait(false);
            await Repository.AddLocation(new Coordinates(89, -179), "another").ConfigureAwait(false);

            var one = await Repository.GetLocations(new Coordinates(89, 1), 4_000, null).ConfigureAwait(false);
            one.Should().HaveCount(1);
            one[0].Address.Should().Be("one");

            var another = await Repository.GetLocations(new Coordinates(89, 179), 4_000, null).ConfigureAwait(false);
            another.Should().HaveCount(1);
            another[0].Address.Should().Be("another");
        }

        [Fact]
        public async Task Only_Closest_Location_Is_Found()
        {
            await Repository.AddLocation(AmsterdamCoordinates, Amsterdam).ConfigureAwait(false);
            await Repository.AddLocation(NewYorkCoordinates, NewYork).ConfigureAwait(false);

            var result = await Repository.GetLocations(HaarlemCoordinates, null, 1).ConfigureAwait(false);

            result.Should().HaveCount(1);
            result[0].Address.Should().Be(Amsterdam);
        }

        private const string Amsterdam = "Amsterdam";
        private static readonly Coordinates AmsterdamCoordinates = new Coordinates(52.3546274, 4.8285836);
        private const string NewYork = "New York";
        private static readonly Coordinates NewYorkCoordinates = new Coordinates(40.6971494, -74.2598643);
        private const string Haarlem = "Haarlem";
        private static readonly Coordinates HaarlemCoordinates = new Coordinates(52.3837711, 4.5728393);

        protected abstract ILocationsRepository Repository { get; }
    }
}
