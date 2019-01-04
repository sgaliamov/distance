using Distance.KdTree;

namespace Distance.Tests
{
    public sealed class LocationsInMemoryRepositoryTests : LocationsRepositoryTests
    {
        protected override ILocationsRepository Repository { get; } = new LocationsInMemoryRepository();
    }
}
