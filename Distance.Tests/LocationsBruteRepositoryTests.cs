using Distance.BruteForce;
using Distance.KdTree;

namespace Distance.Tests
{
    public sealed class LocationsBruteRepositoryTests : LocationsRepositoryTests
    {
        protected override ILocationsRepository Repository { get; } = new LocationsBruteRepository();
    }
}
