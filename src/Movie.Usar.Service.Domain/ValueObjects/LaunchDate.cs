namespace Movie.User.Service.Domain.ValueObjects;

public sealed record LaunchDate(DateTime Value)
{
    public bool IsAfter(DateTime date)
        => Value >= date;
    
    public DateTime Clamp(DateTime date)
        => date < Value ? Value : date;
}