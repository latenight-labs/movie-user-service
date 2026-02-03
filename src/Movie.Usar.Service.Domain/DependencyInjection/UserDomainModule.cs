using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Movie.User.Service.Domain.ValueObjects;


namespace Movie.User.Service.Domain.DependencyInjection;

public static class UserDomainModule
{
    public static IServiceCollection AddUserDomain(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var launchDateValue =
            configuration.GeValue<DateTime>("UserDomain:LaunchDate");

        services.AddSingleton(new LaunchDate(launchDateValue));
        return services;
    }
    
}