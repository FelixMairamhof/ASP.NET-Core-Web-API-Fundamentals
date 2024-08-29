using CityInfoAPI.DbContexts;
using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace CityInfoAPI.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context) 
        { 
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities
                                     .Include(c => c.PointOfInterests)
                                     .Where(c => c.Id == cityId)
                                     .FirstOrDefaultAsync();
            }
            return await _context.Cities
                                 .Where(c => c.Id == cityId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterest
               .Where(p => p.CityId == cityId).ToListAsync();
        }
        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task AddPointOfInterestForCityAsnyc(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if(city != null)
            {
                city.PointOfInterests.Add(pointOfInterest);
            }
            
        }

        public async Task<bool> SaveChangesAsync()
        {
           return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
