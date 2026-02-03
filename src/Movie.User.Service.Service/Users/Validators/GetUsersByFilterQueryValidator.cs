using FluentValidation;
using Microsoft.Extensions.Options;
using Movie.User.Service.Domain.Configuration;
using Movie.User.Service.Service.Users.Queries;
using Movie.User.Service.Service.Users.Validators.Extensions;

namespace Movie.User.Service.Service.Users.Validators;


public class GetUsersByFilterQueryValidator : AbstractValidator<GetUsersByFilterQuery>
{
    public GetUsersByFilterQueryValidator( IOptions<UserDomainOptions> options)
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
                .MinimumLength(rules.Username.Min).WithMessage($"Nome de usuário deve ter no mínimo {rules.Name.Min} caracteres.")
                .MaximumLength(rules.Username.Max).WithMessage($"Nome de usuário deve ter no máximo {rules.Name.Max} caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telefone inválido. Use o formato internacional.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.ZipCode), () =>
        {
            RuleFor(x => x.ZipCode)
                .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP inválido. Use o formato 00000-000 ou 00000000.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.City), () =>
        {
            RuleFor(x => x.City)
                .MinimumLength(rules.City.Min).WithMessage($"Cidade deve ter no mínimo {rules.City.Min} caracteres.")
                .MaximumLength(rules.City.Max).WithMessage($"Cidade deve ter no máximo {rules.City.Max} caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.State), () =>
        {
            RuleFor(x => x.State)
                .MinimumLength(rules.State.Min).WithMessage($"Estado deve ter no mínimo {rules.State.Min} caracteres.")
                .MaximumLength(rules.State.Max).WithMessage($"Estado deve ter no máximo {rules.State.Max} caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Country), () =>
        {
            RuleFor(x => x.Country)
                .MinimumLength(rules.Country.Min).WithMessage($"País deve ter no mínimo {rules.Country.Min} caracteres.")
                .MaximumLength(rules.Country.Max).WithMessage($"País deve ter no máximo {rules.Country.Max} caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Street), () =>
        {
            RuleFor(x => x.Street)
                .MinimumLength(rules.Street.Min).WithMessage($"Endereço deve ter no mínimo {rules.Street.Min} caracteres.")
                .MaximumLength(rules.Street.Max).WithMessage($"Endereço deve ter no máximo {rules.Street.Max} caracteres.");
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
