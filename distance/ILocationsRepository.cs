using System.Threading.Tasks;
using Distance.DataAccess.Entities;

namespace Distance
{
    public interface ILocationsRepository
    {
        Task<LocationEntity[]> GetLocations(double latitude, double longitude, int? maxDistance, int? maxResults);
        Task<long> AddLocation(double latitude, double longitude, string address);
    }
}
