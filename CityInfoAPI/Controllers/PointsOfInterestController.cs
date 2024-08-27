using CityInfoAPI.Models;
using CityInfoAPI.Services;
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
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult <IEnumerable <PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
                var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation("City wasnt found");
                    return NotFound();
                }
                return Ok(city.PointOfInterests);
            }catch(Exception ex)
            {
                _logger.LogCritical("Exception: " + ex );
                return StatusCode(500, "A problem in server");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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

            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(
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
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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

            _mailService.Send("Point of Interst Delete", pointOfInterestFromstore.Name + pointOfInterestFromstore.Description);

            return NoContent();
        }
    }
    
}
