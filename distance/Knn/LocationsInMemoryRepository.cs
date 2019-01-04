using System;
using System.Threading.Tasks;
using Distance.DataAccess.Entities;

namespace Distance.Knn
{
    internal class LocationsInMemoryRepository : ILocationsRepository
    {
        public Task<LocationEntity[]> GetLocations(double latitude, double longitude, int? maxDistance, int? maxResults)
        {
            throw new NotImplementedException();
        }

        public Task<long> AddLocation(double latitude, double longitude, string address)
        {
            throw new NotImplementedException();
        }
    }
}
