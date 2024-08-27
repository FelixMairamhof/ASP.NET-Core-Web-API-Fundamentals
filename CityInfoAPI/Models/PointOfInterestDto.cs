using Microsoft.AspNetCore.Http.HttpResults;

namespace CityInfoAPI.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; }

        public static implicit operator CreatedAtRoute(PointOfInterestDto v)
        {
            throw new NotImplementedException();
        }
    }
}
