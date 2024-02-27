namespace Apartments.Domain.Services.Apartments.Results;

public abstract record ApartmentResult
{
    public static ApartmentResult NotFound() =>
        new ApartmentResultNotFound();

    public static ApartmentResult Ok(Apartment apartment) =>
        new ApartmentResultOk(apartment);

    public static ApartmentResult Ok() =>
        new ApartmentResultOk();
}

public sealed record ApartmentResultNotFound : ApartmentResult;

public sealed record ApartmentResultOk : ApartmentResult
{
    public ApartmentResultOk(Apartment apartment)
    {
        Result = apartment;
    }

    public ApartmentResultOk()
    {
        Result = default;
    }

    public Apartment? Result {
        get;
        init;
    }
}
