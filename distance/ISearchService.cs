using System.Threading.Tasks;
using Distance.Models;

namespace Distance
{
    public interface ISearchService
    {
        Task<SearchResult> GetLocations(Coordinate coordinate, int? maxDistance, int? maxResults);
    }
}