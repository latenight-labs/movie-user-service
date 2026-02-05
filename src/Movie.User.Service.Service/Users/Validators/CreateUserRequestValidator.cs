using FluentValidation;
using Microsoft.Extensions.Options;
using Movie.User.Service.Domain.Configuration;
using Movie.User.Service.Service.Users.DTOs;
using Movie.User.Service.Service.Users.Validators.Extensions;

namespace Movie.User.Service.Service.Users.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IOptions<UserDomainOptions> options)
    {
        var rules = options.Value;

        RuleFor(x => x.Name)
            .RequiredWithLength(rules.Name.Min, rules.Name.Max, "Nome");

        RuleFor(x => x.Username)
            .RequiredWithLength(rules.Username.Min, rules.Username.Max, "Nome de Usuário");

        RuleFor(x => x.Email)
            .Required("Email")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Phone)
            .MatchesPhoneRegex(rules.PhoneRegex, "Telefone");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Endereço é obrigatório.");

        RuleFor(x => x.Address.Street)
            .RequiredWithLength(rules.Street.Min, rules.Street.Max, "Rua");

        RuleFor(x => x.Address.City)
            .RequiredWithLength(rules.City.Min, rules.City.Max, "Cidade");

        RuleFor(x => x.Address.State)
            .RequiredWithLength(rules.State.Min, rules.State.Max, "Estado");

        RuleFor(x => x.Address.ZipCode)
            .MatchesZipCodeRegex(rules.ZipCodeRegex, "CEP");

        RuleFor(x => x.Address.Country)
            .RequiredWithLength(rules.Country.Min, rules.Country.Max, "País");
    }
}
