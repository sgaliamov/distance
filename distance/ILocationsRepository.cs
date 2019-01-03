using System.Threading.Tasks;
using Distance.DataAccess.Entities;

namespace Distance
{
    public interface ILocationsRepository
    {
        Task<LocationEntity[]> GetLocations(double longitude, double latitude, int? maxDistance, int? maxResults);
        Task<long> CreateLocation(double longitude, double latitude, string address);
    }
}
