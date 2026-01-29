using FluentValidation;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Service.Users.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(2).WithMessage("Nome deve ter no mínimo 2 caracteres.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório.")
            .MinimumLength(3).WithMessage("Nome de usuário deve ter no mínimo 3 caracteres.")
            .MaximumLength(50).WithMessage("Nome de usuário deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telefone inválido.");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Endereço é obrigatório.");

        RuleFor(x => x.Address.Street)
            .NotEmpty().WithMessage("Rua é obrigatória.");

        RuleFor(x => x.Address.City)
            .NotEmpty().WithMessage("Cidade é obrigatória.");

        RuleFor(x => x.Address.State)
            .NotEmpty().WithMessage("Estado é obrigatório.");

        RuleFor(x => x.Address.ZipCode)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP inválido.");

        RuleFor(x => x.Address.Country)
            .NotEmpty().WithMessage("País é obrigatório.");
    }
}
