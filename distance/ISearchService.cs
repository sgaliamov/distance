using System.Threading.Tasks;
using Distance.Models;

namespace Distance
{
    public interface ISearchService
    {
        Task<SearchResult> GetLocations(Coordinates coordinates, int? maxDistance, int? maxResults);
    }
}
