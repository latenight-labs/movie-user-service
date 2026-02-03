using FluentValidation;
using Microsoft.Extensions.Options;
using Movie.User.Service.Domain.Configuration;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Service.Users.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IOptions<UserDomainOptions> options)
    {
        var rules = options.Value;
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(rules.Name.Min).WithMessage($"Nome deve ter no mínimo {rules.Name.Min} caracteres.")
            .MaximumLength(rules.Name.Max).WithMessage($"Nome deve ter no máximo {rules.Name.Max} caracteres.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório.")
            .MinimumLength(rules.Username.Min).WithMessage($"Nome de usuário deve ter no mínimo {rules.Username.Min} caracteres.")
            .MaximumLength(rules.Username.Max).WithMessage($"Nome de usuário deve ter no máximo {rules.Username.Max} caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telefone inválido.");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Endereço é obrigatório.");

        RuleFor(x => x.Address.Street)
            .MinimumLength(rules.Street.Min).WithMessage($"Rua deve ter no mínimo {rules.Street.Min} caracteres.")
            .MaximumLength(rules.Street.Max).WithMessage($"Rua deve ter no máximo {rules.Street.Max} caracteres.")
            .NotEmpty().WithMessage("Rua é obrigatória.");

        RuleFor(x => x.Address.City)
            .MinimumLength(rules.City.Min).WithMessage($"Cidade deve ter no mínimo {rules.City.Min} caracteres.")
            .MaximumLength(rules.City.Max).WithMessage($"Cidade deve ter no máximo {rules.City.Max} caracteres.")
            .NotEmpty().WithMessage("Cidade é obrigatória.");

        RuleFor(x => x.Address.State)
            .MinimumLength(rules.State.Min).WithMessage($"Estado deve ter no mínimo {rules.State.Min} caracteres.")
            .MaximumLength(rules.State.Max).WithMessage($"Estado deve ter no máximo {rules.State.Max} caracteres.")
            .NotEmpty().WithMessage("Estado é obrigatório.");

        RuleFor(x => x.Address.ZipCode)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP inválido.");

        RuleFor(x => x.Address.Country)
            .MinimumLength(rules.Country.Min).WithMessage($"País deve ter no mínimo {rules.Country.Min} caracteres.")
            .MaximumLength(rules.Country.Max).WithMessage($"Pais deve ter no máximo {rules.Country.Max} caracteres.")
            .NotEmpty().WithMessage("País é obrigatório.");
    }
}
