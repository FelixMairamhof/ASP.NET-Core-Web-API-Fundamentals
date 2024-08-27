using CityInfoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/pointofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public ActionResult <IEnumerable <PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault( c => c.Id == cityId);
            if (city == null)
            {
                _logger.LogInformation("City wasnt found");
                return NotFound();
            }
            return Ok(city.PointOfInterests);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            
            var pointOfInterest = city.PointOfInterests
                .FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);
        }


        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        { 

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                cityId => city.PointOfInterests).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointOfInterests.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
               new
               {
                   cityId = cityId,
                   pointOfInterestId = finalPointOfInterest.Id
               }, finalPointOfInterest
            );

        }
        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromstore = city.PointOfInterests
                    .FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestFromstore == null)
            {
                return NotFound();
            }
            pointOfInterestFromstore.Name = pointOfInterest.Name;
            pointOfInterestFromstore.Description = pointOfInterest.Description;

            return NoContent();

        }
        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromstore = city.PointOfInterests
                    .FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestFromstore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromstore.Name,
                    Description = pointOfInterestFromstore.Description,
                };
            patchDocument.ApplyTo(pointOfInterestToPatch,ModelState );

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromstore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromstore.Description = pointOfInterestToPatch.Description;

            return NoContent();

        }
        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest( int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromstore = city.PointOfInterests
                    .FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestFromstore == null)
            {
                return NotFound();
            }
            city.PointOfInterests.Remove(pointOfInterestFromstore);
            return NoContent();
        }
    }
    
}
