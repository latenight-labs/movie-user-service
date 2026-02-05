using FluentValidation;
using Microsoft.Extensions.Options;
using Movie.User.Service.Domain.Configuration;
using Movie.User.Service.Service.Users.Queries;
using Movie.User.Service.Service.Users.Validators.Extensions;

namespace Movie.User.Service.Service.Users.Validators;

/// <summary>
/// Validator for GetUsersByFilterQuery using FluentValidation
/// </summary>
public class GetUsersByFilterQueryValidator : AbstractValidator<GetUsersByFilterQuery>
{
    /// <summary>
    /// Initializes a new instance of the GetUsersByFilterQueryValidator with user domain options.
    /// </summary>
    /// <param name="options">The user domain options containing validation rules.</param>
    public GetUsersByFilterQueryValidator(IOptions<UserDomainOptions> options)
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

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
                .Matches(rules.PhoneRegex).WithMessage("Telefone inválido. Use o formato internacional.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.ZipCode), () =>
        {
            RuleFor(x => x.ZipCode)
                .Matches(rules.ZipCodeRegex).WithMessage("CEP inválido. Use o formato 00000-000 ou 00000000.");
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

        When(x => !string.IsNullOrWhiteSpace(x.Street), () =>
        {
            RuleFor(x => x.Street)
                .WithMinAndMaxLength(rules.Street.Min, rules.Street.Max, "Endereço");
        });
        When(x => x.StartDate.HasValue, () => // Arthur
        {
            RuleFor(x => x.StartDate)
                .InclusiveBetween(rules.LaunchDate, DateTime.Today)
                .WithMessage($"Data deve estar entre {rules.LaunchDate} e hoje.");
        });
    }

    private bool HaveAtLeastOneFilter(GetUsersByFilterQuery query)
    {
        return !string.IsNullOrWhiteSpace(query.Username) ||
               !string.IsNullOrWhiteSpace(query.Phone) ||
               !string.IsNullOrWhiteSpace(query.Street) ||
               !string.IsNullOrWhiteSpace(query.City) ||
               !string.IsNullOrWhiteSpace(query.State) ||
               !string.IsNullOrWhiteSpace(query.ZipCode) ||
               !string.IsNullOrWhiteSpace(query.Country) ||
               !query.StartDate.HasValue;// Arthur
    }
}
