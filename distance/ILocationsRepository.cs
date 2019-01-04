using System.Threading.Tasks;
using Distance.Models;

namespace Distance
{
    public interface ILocationsRepository
    {
        Task<Location[]> GetLocations(double latitude, double longitude, int? maxDistance, int? maxResults);
        Task<long> AddLocation(double latitude, double longitude, string address);
    }
}
