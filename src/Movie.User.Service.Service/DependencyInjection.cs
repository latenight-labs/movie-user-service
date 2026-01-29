using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.SearchStrategies;
using Movie.User.Service.Service.Users.Validators;
using System.Reflection;

namespace Movie.User.Service.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR - Handlers para Commands e Queries
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // FluentValidation - Validadores
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Pipeline Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Search Strategies - Strategy Pattern para busca de usuários
        services.AddScoped<IUserSearchStrategy, AllUsersSearchStrategy>();
        services.AddScoped<IUserSearchStrategy, UsernameSearchStrategy>();
        services.AddScoped<IUserSearchStrategy, CitySearchStrategy>();
        services.AddScoped<IUserSearchStrategy, StateSearchStrategy>();
        services.AddScoped<IUserSearchStrategy, CountrySearchStrategy>();
        services.AddScoped<IUserSearchStrategy, AddressSearchStrategy>();
        services.AddScoped<IUserSearchStrategy, PhoneSearchStrategy>();
        services.AddScoped<IUserSearchStrategy, ZipCodeSearchStrategy>();

        return services;
    }
}
