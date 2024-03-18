using Apartments.Application.Modules.Apartments;
using Apartments.Application.Modules.Apartments.Dtos;
using Apartments.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

/// <summary>
/// Controller for managing apartments.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ApartmentsController(IApartmentService apartmentService)
    : ControllerBase
{
    /// <summary>
    /// Get all apartments.
    /// </summary>
    /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
    /// <returns>A list of apartments.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetAsync(CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound();
    }
    
    /// <summary>
    /// Get a specific apartment by its ID.
    /// </summary>
    /// <param name="id">The ID of the apartment.</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
    /// <returns>The apartment with the given ID.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApartmentDto>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound();
    }

    /// <summary>
    /// Create a new apartment.
    /// </summary>
    /// <param name="request">The request containing the details of the apartment to be created.</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
    /// <returns>The result of the creation operation.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PostAsync([FromBody] ApartmentRequest request, CancellationToken cancellationToken)
    {
        var dto = new CreateApartmentCommand
        {
            Name = request.Name,
            Address = new CreateApartmentAddressDto
            {
                Street = request.Street,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                BuildingNo = request.BuildingNumber,
                FlatNumber = request.FlatNumber
            }
        };

        var result = await apartmentService.CreateAsync(dto, cancellationToken);

        return result.IsSuccess
            ? CreatedAtRoute(nameof(GetAsync), routeValues: new { id = result.Value }, value: null)
            : BadRequest(result.Errors);
    }

    /// <summary>
    /// Update an existing apartment.
    /// </summary>
    /// <param name="id">The ID of the apartment to be updated.</param>
    /// <param name="request">The request containing the new details of the apartment.</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
    /// <returns>The result of the update operation.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutAsync(Guid id, [FromBody] UpdateApartmentRequest request, CancellationToken cancellationToken)
    {
        var dto = new UpdateApartmentCommand
        {
            Id = id,
            Name = request.Name
        };

        var result = await apartmentService.UpdateApartment(dto, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : NotFound();
    }

    /// <summary>
    /// Delete an apartment.
    /// </summary>
    /// <param name="id">The ID of the apartment to be deleted.</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await apartmentService.DeleteAsync(id, cancellationToken);
            
        return NoContent();
    }
}
