using FluentValidation;
using Microsoft.Extensions.Options;
using Movie.User.Service.Api.Requests;
using Movie.User.Service.Domain.Configuration;
using Movie.User.Service.Service.Users.Validators.Extensions;

namespace Movie.User.Service.Api.Validators;

/// <summary>
/// Validador para SearchUsersRequest usando FluentValidation
/// </summary>
public class SearchUsersRequestValidator : AbstractValidator<SearchUsersRequest>
{
    /// <summary>
    /// Initializes a new instance of the SearchUsersRequestValidator with user domain options.
    /// </summary>
    /// <param name="options">The user domain options containing validation rules.</param>
    public SearchUsersRequestValidator(IOptions<UserDomainOptions> options)
    {
        var rules = options.Value;

        // Valida que pelo menos um filtro seja fornecido
        RuleFor(x => x)
            .Must(HaveAtLeastOneFilter)
            .WithMessage("Pelo menos um filtro deve ser fornecido para a busca.");

        // Validações opcionais para cada campo (quando fornecido)
        When(x => !string.IsNullOrWhiteSpace(x.Username), () =>
        {
            RuleFor(x => x.Username)
                .WithMinAndMaxLength(rules.Username.Min, rules.Username.Max, "Nome de usuário");
        });


        When(x => !string.IsNullOrWhiteSpace(x.City), () =>
        {
            RuleFor(x => x.City)
                .WithMinAndMaxLength(rules.City.Min, rules.City.Max, "Cidade");
        });

        When(x => !string.IsNullOrWhiteSpace(x.State), () =>
        {
            RuleFor(x => x.State)
                .WithMinAndMaxLength(rules.State.Min, rules.State.Max, "Estado");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Country), () =>
        {
            RuleFor(x => x.Country)
                .WithMinAndMaxLength(rules.Country.Min, rules.Country.Max, "País");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .WithMinAndMaxLength(rules.Name.Min, rules.Name.Max, "Nome");
        });
        When(x => x.StartDate.HasValue, () =>
        {
            RuleFor(x => x.StartDate)
                .InclusiveBetween(rules.LaunchDate, DateTime.Today)
                .WithMessage($"Data deve estar entre {rules.LaunchDate:dd/MM/yyyy} e hoje.");
        });
    }

    private bool HaveAtLeastOneFilter(SearchUsersRequest request)
    {
        return !string.IsNullOrWhiteSpace(request.Name) ||
               !string.IsNullOrWhiteSpace(request.Username) ||
               !string.IsNullOrWhiteSpace(request.City) ||
               !string.IsNullOrWhiteSpace(request.State) ||
               !string.IsNullOrWhiteSpace(request.Country) ||
               !request.StartDate.HasValue;
    }
}
