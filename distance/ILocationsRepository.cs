using System.Threading.Tasks;
using Distance.Models;

namespace Distance
{
    public interface ILocationsRepository
    {
        Task<Location[]> GetLocations(Coordinates coordinates, int? maxDistance, int? maxResults);
        Task<long> AddLocation(Coordinates coordinates, string address);
    }
}
