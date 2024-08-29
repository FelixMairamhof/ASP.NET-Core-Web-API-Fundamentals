using CityInfoAPI.Entities;

namespace CityInfoAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<City?> GetCityAsync(int cityid, bool includePointsOfInterest);

        Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

        Task<bool> CityExistsAsync(int cityId);

        Task AddPointOfInterestForCityAsnyc(int cityId, PointOfInterest pointOfInterest);

        Task<bool> SaveChangesAsync();

        void DeletePointOfInterest ( PointOfInterest pointOfInterest );

        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10);
    }
}
