using Apartments.Application.Apartments;
using Apartments.Domain.Services.Apartments.Dtos;
using Apartments.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ApartmentsController(ApartmentService apartmentService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Errors);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(id, cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ApartmentRequest request, CancellationToken cancellationToken)
    {
        var dto = new CreateApartmentDto
        {
            Name = request.Name,
            Address = new CreateApartmentAddressDto()
            {
                Street = request.Street,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                BuildingNo = request.BuildingNumber,
                FlatNumber = request.FlatNumber,
            }
        };
        
        var result = await apartmentService.CreateApartment(dto, cancellationToken);

        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetAsync), new { id = result.Value }, null) 
            : BadRequest(result.Errors);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateApartmentRequest request, CancellationToken cancellationToken)
    {
        var dto = new UpdateApartmentDto
        {
            Id = id,
            Name = request.Name
        };
        
        var result = await apartmentService.UpdateApartment(dto, cancellationToken);
            
        if (result.IsSuccess)
        {
            return NoContent();
        }
            
        return NotFound(result.Errors);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await apartmentService.DeleteAsync(id, cancellationToken);
            
        return NoContent();
    }
}
