using CityInfoAPI.Models;

namespace CityInfoAPI
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CitiesDataStore Current { get; set; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park.",
                    PointOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "A large public park in New York City."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "A 102-story skyscraper in Midtown Manhattan."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    PointOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Eiffel Tower",
                            Description = "A wrought-iron lattice tower on the Champ de Mars."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "Louvre Museum",
                            Description = "The world's largest art museum."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Tokyo",
                    Description = "The one with that busy crossing.",
                    PointOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 5,
                            Name = "Shibuya Crossing",
                            Description = "A famous pedestrian crossing in Tokyo."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 6,
                            Name = "Tokyo Tower",
                            Description = "A communications and observation tower in the Shiba-Koen district."
                        }
                    }
                }
            };
        }
    }
}
