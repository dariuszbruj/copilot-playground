using Apartments.Application.Modules.Utilities;
using Apartments.Domain.Models;
using Apartments.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers
{
    
    /// <summary>
    /// Controller for managing utility measurements.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
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
        /// <param name="request">The utility measurement request.</param>
        /// <returns>An IActionResult that represents the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> InsertUtilityValues([FromBody] UtilityMeasurementRequest request)
        {
            var utilityMeasurement = new UtilityMeasurement
            {
                ApartmentId = request.ApartmentId,
                MeasurementDate = request.MeasurementDate,
                UtilityUsage = request.UtilityUsage,
                UtilityType = (UtilityType)Enum.Parse(typeof(UtilityType), request.UtilityType.ToString())
            };

            await _utilityService.InsertUtilityValues(utilityMeasurement);
            
            return Ok();
        }
    }
}
