using System.Threading.Tasks;
using Distance.Models;

namespace Distance
{
    public interface ISearchService
    {
        Task<SearchResult> GetLocations(Location location, int? maxDistance, int? maxResults);
    }
}