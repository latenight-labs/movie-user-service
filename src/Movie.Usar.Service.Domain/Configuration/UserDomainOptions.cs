namespace Movie.User.Service.Domain.Configuration;

public class UserDomainOptions
{
    
    public LengthRule Name { get; init; } = new();
    public LengthRule Username { get; init; } = new();
    public LengthRule Email { get; init; } = new();
    
    public string PhoneRegex { get; init; } = string.Empty;
    
    //ADRESS
    public string ZipCodeRegex { get; init; } = string.Empty;
    public LengthRule Street { get; init; } = new();
    public LengthRule City { get; init; } = new();
    public LengthRule State { get; init; } = new();
    public LengthRule Country { get; init; } = new();
    
    
    public DateTime LaunchDate { get; init; }

}
