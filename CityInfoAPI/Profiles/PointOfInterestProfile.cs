using AutoMapper;

namespace CityInfoAPI.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestDto, Entities.PointOfInterest>();
        }
    }
}
