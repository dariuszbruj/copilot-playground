using Apartments.Application.Modules.Utilities;
using Apartments.Application.Modules.Utilities.Dtos;
using Apartments.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers
{
    
    /// <summary>
    /// Controller for managing utility measurements.
    /// </summary>
    [ApiController]
    [Route("apartments/{apartmentId:guid}/utilities")]
    [Authorize]
    public class UtilityController : ControllerBase
    {
        private readonly IUtilityService _utilityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UtilityController"/> class.
        /// </summary>
        /// <param name="utilityService">The utility service.</param>
        public UtilityController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        /// <summary>
        /// Inserts utility values.
        /// </summary>
        /// <param name="apartmentId">Apartment id</param>
        /// <param name="request">The utility measurement request.</param>
        /// <returns>An IActionResult that represents the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> InsertUtilityValues(Guid apartmentId, [FromBody] UtilityMeasurementRequest request)
        {
            var utilityMeasurementDto = new UtilityMeasurementDto
            {
                ApartmentId = apartmentId,
                MeasurementDate = request.MeasurementDate,
                UtilityUsage = request.UtilityUsage,
                UtilityType = (UtilityTypeDto)Enum.Parse(typeof(UtilityTypeDto), request.UtilityType.ToString())
            };

            await _utilityService.InsertUtilityValues(utilityMeasurementDto);
            
            return Ok();
        }
    }
}
