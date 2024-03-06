using Apartments.Domain.Services.Apartments;
using Apartments.Domain.Services.Apartments.Dtos;
using Apartments.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ApartmentsController(IApartmentService apartmentService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(cancellationToken);

        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : Results.NotFound(result.Errors);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await apartmentService.GetAsync(id, cancellationToken);

        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : Results.NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IResult> PostAsync([FromBody] ApartmentRequest request, CancellationToken cancellationToken)
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
            ? Results.CreatedAtRoute(nameof(GetAsync), new { id = result.Value }) 
            : Results.BadRequest(result.Errors);
    }

    [HttpPut("{id:guid}")]
    public async Task<IResult> PutAsync(Guid id, [FromBody] UpdateApartmentRequest request, CancellationToken cancellationToken)
    {
        var dto = new UpdateApartmentDto
        {
            Id = id,
            Name = request.Name
        };
        
        var result = await apartmentService.UpdateApartment(dto, cancellationToken);
            
        return result.IsSuccess 
            ? Results.NoContent() 
            : Results.NotFound(result.Errors);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await apartmentService.DeleteAsync(id, cancellationToken);
            
        return Results.NoContent();
    }
}
