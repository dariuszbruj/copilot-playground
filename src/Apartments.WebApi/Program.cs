using Apartments.Application.Apartments;
using Apartments.Infrastructure.EntityFramework;
using Apartments.Infrastructure.Identity;
using Apartments.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddApartmentsModule();

builder.Services.AddControllers( x => 
    x.SuppressAsyncSuffixInActionNames = false
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
