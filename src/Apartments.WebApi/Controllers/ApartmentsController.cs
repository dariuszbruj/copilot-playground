using Apartments.Application.Modules.Apartments;
using Apartments.Application.Modules.Apartments.Dtos;
using Apartments.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ApartmentsController(IApartmentService apartmentService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetAsync(CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApartmentDto>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(id, cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound();
    }

    [HttpPost]
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

    [HttpPut("{id:guid}")]
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

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await apartmentService.DeleteAsync(id, cancellationToken);
            
        return NoContent();
    }
}
