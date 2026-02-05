namespace Movie.User.Service.Domain.Configuration;

public class UserDomainOptions
{
    
    public LengthRule Name { get; init; } = default!;
    public LengthRule Username { get; init; } = default!;
    public LengthRule Email { get; init; } = default!;
    
    public string PhoneRegex { get; init; } = default!;
    
    //ADRESS
    public string ZipCodeRegex { get; init; } = default!;
    public LengthRule Street { get; init; } = default!;
    public LengthRule City { get; init; } = default!;
    public LengthRule State { get; init; } = default!;
    public LengthRule Country { get; init; } = default!;
    
    
    public DateTime LaunchDate { get; init; }

}
